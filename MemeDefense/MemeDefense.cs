using System;

using SharpKit.JavaScript;
using SharpKit.Html4;
using SharpKit.jQuery;

using WebDE;
using WebDE.GameObjects;
using WebDE.Animation;
using WebDE.Rendering;
using WebDE.GUI;
using WebDE.InputManager;
using WebDE.AI;

namespace MemeDefense
{
    [JsType(JsMode.Global, Filename = "build/scripts/MemeDefense.js")]
    public class Globals// : jQueryContextBase
    {
        public static void Initialize()
        {
            new MemeDefense();
        }
    }

    [JsType(JsMode.Clr, Filename = "build/scripts/MemeDefense.js")]
    public partial class MemeDefense
    {
        //a placeholder for which tower we intend to place based on the clicked gui element
        private LivingGameEntity towerToPlace = null;
        //the (enemy) exit point for the level
        private GameEntity levelExitPoint;
        private string desiredTowerName;
        
        //entrance point for standard rageface enemies
        private GameEntitySpawner levelEntryPoint;
        //different entrance point for boss(es)?

        private Notification placeTower;

        private ArtificialIntelligence enemyAI = new ArtificialIntelligence();

        // Player variables
        private int playerCurrency = 333;
        private int playerHealth = 100;
        private Faction playerFaction = new Faction("Player", Color.White);
        private Faction enemyFaction = new Faction("Enemy", Color.Black);

        public MemeDefense()
        {
            Debug.showDebug = false;
            Main.Initialize();
            View.GetMainView().SetLightingStyle(LightingStyle.None);

            prepSprites();
            prepResources();
            prepTowers();

            //load the level...
            //CreateTestArea();
            CreatePastryLevel();
            //CreateTestLevel();

            //create the GUI menu for tower building
            createGUI();

            prepEnemies();
        }

        private void prepResources()
        {
            new Resource("Bacon");
        }

        public void prepTowers()
        {
            LivingGameEntity NyanTower = new LivingGameEntity("NyanTower");
            LivingGameEntity XTheYTower = new LivingGameEntity("XTheYTower");
            LivingGameEntity TableFlipperTower = new LivingGameEntity("TableFlipperTower");
            LivingGameEntity BarrelRollTower = new LivingGameEntity("BarrelRollTower");
            LivingGameEntity ArrowKneeTower = new LivingGameEntity("GuardTower");

            LivingGameEntity PepperCopTower = new LivingGameEntity("PepperCopTower");

            //NyanTower.SetResource("Currency", 15);
            //PepperCopTower.SetResource("Currency", 35);
            //XTheYTower.SetResource("Currency", 40);
            //TableFlipperTower.SetResource("Currency", 20);

            //this will automatically draw the sprite for the projectile from the (hopefully) pre-established table sprite
            Projectile TableProjectile = new Projectile("Table");
            TableProjectile.SetDamage(30);
            
            Weapon TableFlipperWeapon = new Weapon(10, 1, .5, 180, 40);
            TableFlipperWeapon.SetProjectile(TableProjectile);
            TableFlipperWeapon.SetRange(5);
            TableFlipperTower.AddWeapon(TableFlipperWeapon);

            Weapon dummyWeapon = new Weapon(10, 1, .5, 180, 40);
            dummyWeapon.SetRange(5);
            NyanTower.AddWeapon(dummyWeapon);

            Projectile barrel = new Projectile("Barrel");
            barrel.SetDamage(70);

            Weapon BarrelThrower = new Weapon(10, 1, .5, 180, 40);
            BarrelThrower.SetRange(5);
            BarrelThrower.SetProjectile(barrel);
            BarrelRollTower.AddWeapon(BarrelThrower);

            Projectile arrow = new Projectile("Arrow");
            arrow.SetDamage(70);

            Weapon Bow = new Weapon(10, 1, .5, 180, 40);
            Bow.SetRange(15);
            Bow.SetProjectile(arrow);
            ArrowKneeTower.AddWeapon(Bow);
        }

        #region gui;

        private void createGUI()
        {
            //createCursorLayer();

            createLevelSelectMenu();

            createTowerMenu();

            createHUD();

            createTopBar();
        }

        private void createCursorLayer()
        {
            // Create the cursor layer.
            GuiLayer cursorLayer = View.GetMainView().AddLayer("CursorLayer", new Rectangle(0, 0, 100, 100));
            cursorLayer.FollowCursor(true);
            cursorLayer.Deactivate();

            //add content to the cursor layer
            GuiElement cursorIcon = cursorLayer.AddGUIElement("");
            cursorIcon.SetSize(40, 40);

            cursorLayer.Hide();
        }

        // Set the cursor to the specified sprite. If null is passed, reset the cursor to arrow.
        public void SetCursor(Sprite newCursor)
        {
            // Use custom JS & CSS.
            // http://www.w3schools.com/cssref/pr_class_cursor.asp

            //GuiLayer.GetLayerByName("CursorLayer").GetElementAt(0, 0).SetSprite(newCursor);
            //GuiLayer.GetLayerByName("CursorLayer").Show();
        }

        public void HideCursor()
        {
            //GuiLayer.GetLayerByName("CursorLayer").Hide();
        }

        private void createTopBar()
        {
            GuiLayer topBar = View.GetMainView().AddLayer("TopBar", new Rectangle(0, 0, 200, 40));
            //topBar.SetPosition(Game.GetRenderer().GetSize().width / 2 - 100, Game.GetRenderer().GetSize().height - topBar.GetSize().height);
            topBar.SetPosition(Game.GetRenderer().GetSize().width / 2 - 100, 0);

            // Wave GUI
            // The current wave we're on.
            LabelValue currentWave = new LabelValue(topBar, "Wave", "0");
            currentWave.SetPosition(0, 0);
            // The total number of entities in the wave.
            LabelValue totalWaves = new LabelValue(topBar, "/", "0");
            totalWaves.SetPosition(60, 0);

            // Which number in the wave is currently spawning.
            LabelValue wavePosition = new LabelValue(topBar, "Enemy", "0");
            wavePosition.SetPosition(100, 0);
            // The number of entities in the wave.
            LabelValue waveCount = new LabelValue(topBar, "/", "0");
            waveCount.SetPosition(160, 0);
        }

        //create the GUI menu for building towers
        private void createTowerMenu()
        {
            //create the gui functions
            GUIFunction gfAction = new GUIFunction("Action", InputDevice.Mouse, "mouse0");
            GUIFunction gfContext = new GUIFunction("Context", InputDevice.Mouse, "mouse1");
            //InputDevice.Keyboard.Bind("enter", 0, gfAction);

            //create the layer itself
            GuiLayer towerMenu = View.GetMainView().AddLayer("TowerMenu", new Rectangle(-100, 40, 146, 300));
            //make this the active UI layer
            towerMenu.Activate();

            GuiElement label = towerMenu.AddGUIElement("Towers");
            label.SetPosition(0, 0);
            //create a GUI element to place a tower
            GuiElement nyanTower = towerMenu.AddGUIElement("Nyan Nest");
            //give it a single frame sprite for the tower icon
            nyanTower.SetSprite(Sprite.GetSpriteByName("NyanTower"));
            nyanTower.SetPosition(70, 32);
            //call the animation call once to get it to render
            nyanTower.SetCustomValue("NyanTower");
            //repeat for the other towers

            GuiElement xTheYTower = towerMenu.AddGUIElement("All The Things");
            xTheYTower.SetSprite(Sprite.GetSpriteByName("XTheYTower"));
            xTheYTower.SetPosition(20, 132);
            xTheYTower.SetCustomValue("XTheYTower");

            GuiElement tableFlipperTower = towerMenu.AddGUIElement("Table Flipper");
            tableFlipperTower.SetSprite(Sprite.GetSpriteByName("TableFlipperTower"));
            tableFlipperTower.SetPosition(20, 32);
            tableFlipperTower.SetCustomValue("TableFlipperTower");

            GuiElement barrelRollTower = towerMenu.AddGUIElement("Barrel Roll Tower");
            barrelRollTower.SetSprite(Sprite.GetSpriteByName("BarrelRollTower"));
            barrelRollTower.SetPosition(20, 82);
            barrelRollTower.SetCustomValue("BarrelRollTower");

            GuiElement arrowKneeTower = towerMenu.AddGUIElement("Guard Tower");
            arrowKneeTower.SetSprite(Sprite.GetSpriteByName("GuardTower"));
            arrowKneeTower.SetPosition(70, 82);
            arrowKneeTower.SetCustomValue("GuardTower");

            // Assign the click event for each button (tower) in the menu to the function that handles clicking for this menu.
            foreach (GuiElement towerIcon in towerMenu.GetGuiElements())
            {
                towerIcon.SetGUIFunction(gfAction, towerMenu_Click);
            }
        }

        private void createLevelSelectMenu()
        {
            //create the gui functions
            //GUIFunction gfAction = new GUIFunction("Action", InputDevice.Mouse, "mouse0");
            GUIFunction gfAction = GUIFunction.GetByName("Action");

            //create the layer itself
            GuiLayer levelMenu = View.GetMainView().AddLayer("LevelMenu", new Rectangle(100, 40, 146, 200));

            GuiElement testButton = levelMenu.AddGUIElement("Test Level");
            testButton.SetPosition(0, 0);
            //towerIcon.SetGUIFunction(gfAction, towerMenu_Click);
            GuiElement pastryButton = levelMenu.AddGUIElement("Pastry Level");
            pastryButton.SetPosition(0, 20);
            GuiElement hyperboleButton = levelMenu.AddGUIElement("Hyperbole Level");
            hyperboleButton.SetPosition(0, 40);
            GuiElement kickstarterButton = levelMenu.AddGUIElement("Kickstarter Level");
            kickstarterButton.SetPosition(0, 60);
            GuiElement ikeaButton = levelMenu.AddGUIElement("Ikea Level");
            ikeaButton.SetPosition(0, 80);
        }

        // Functionless information readout. Does nothing but report information to the player.
        private void createHUD()
        {
            //create the layer itself
            GuiLayer hud = View.GetMainView().AddLayer("HUD", new Rectangle(-100, -100, 146, 100));

            LabelValue currency = new LabelValue(hud, "Bacon", playerCurrency.ToString());
            currency.SetPosition(0, 0);

            LabelValue health = new LabelValue(hud, "Karma", playerHealth.ToString());
            health.SetPosition(0, 20);
        }

        public void towerMenu_Click(GuiEvent gEvent)
        {
            //set up the tower that we intend to click based on the custom value set in the gui element
            desiredTowerName = gEvent.clickedElement.GetCustomValue();
            if (desiredTowerName == "" || desiredTowerName == null)
            {
                //Debug.log(gEvent.clickedElement.GetId() + " has no custom value :(");
                return;
            }

            //attach a function to the main view when the user clicks on it
            GUIFunction gfAction = GUIFunction.GetByName("Action");
            Stage.CurrentStage.GetCollisionMap().SetGUIFunction(gfAction, placeATower);

            //show the collision map on the main playable area
            Stage.CurrentStage.ShowCollisionMap();

            //set the gui element that led us here to be highlighted
            gEvent.clickedElement.Select(true);

            //set the tower menu as the default gui layer, so that if something tries to switch to default in the future, it switches to this
            //deactivate the tower menu gui layer

            //create a gui element and set it to following the mouse
            Debug.log("Attempting to set cursor to sprite " + desiredTowerName);
            SetCursor(Sprite.GetSpriteByName(desiredTowerName));

            //throw up a notification for the player
            Sprite ggg = Sprite.GetSpriteByName("GoodGuyGreg");
            placeTower = new Notification(ggg, "Good Guy Greg", "@ggg", "Click where you would like the tower to be placed.", 10);
        }

        //test function to make a new tower...
        public void placeATower(GuiEvent gEvent)
        {
            //hide the collision map gui layer
            Stage.CurrentStage.HideCollisionMap();

            // Make sure we can place a tower there.
            if (checkTowerPlacement(gEvent) == false)
            {
                // Make a noise.
                return;
            }

            // Need to add a check for financial ability as well ... 
            // But maybe on the tower menu instead.

            Tile clickedTile = gEvent.clickedTiles[0];

            // Decrement the player's currency
            changePlayerCurrency(-33);

            towerToPlace = LivingGameEntity.CloneByName(desiredTowerName);
            towerToPlace.SetParentStage(Stage.CurrentStage);
            towerToPlace.SetPosition(clickedTile.GetPosition().x, clickedTile.GetPosition().y);
            towerToPlace.Faction = playerFaction;

            Stage.CurrentStage.AddLivingGameEntity(towerToPlace);

            //deselect the selected tower in the gui menu
            GuiLayer towerLayer = GuiLayer.GetLayerByName("TowerMenu");
            //towerLayer.SelectItem("By putting this text here, everything will be deselected, but nothing will be selected.", -1);
            // Destroy the notification.
            placeTower.Destroy();

            // After the tower is placed, re-calculate pathing for the stage.
            // If there is only one path enemies can take, mark the crucial point(s) as walkable but unbuildable.
        }

        // Returns true if a tower can be placed at the tile located at the gui event. False if a tower cannot be placed.
        private bool checkTowerPlacement(GuiEvent gEvent)
        {
            if (gEvent.clickedTiles.Count == 0 || desiredTowerName == "")
            {
                Debug.log("Can't do click. No tile or no tower. Returning.");
                return false;
            }
            Tile clickedTile = gEvent.clickedTiles[0];

            if (clickedTile.GetBuildable() == false)
            {
                return false;
            }

            return true;
        }

        private void updateWaveGUI(GameEntitySpawner spawner, GameEntity gent)
        {
            GuiLayer topBar = GuiLayer.GetLayerByName("TopBar");

            // The current wave we're on.
            topBar.GetGUIElement(0).As<LabelValue>().SetValue(spawner.GetCurrentBatch());
            // The total number of entities in the wave.
            topBar.GetGUIElement(1).As<LabelValue>().SetValue(spawner.GetBatchCount());
            // Which number in the wave is currently spawning.
            topBar.GetGUIElement(2).As<LabelValue>().SetValue(spawner.GetCurrentSpawnPosition());
            // The number of entities in the wave.
            topBar.GetGUIElement(3).As<LabelValue>().SetValue(spawner.GetCurrentSpawnCount());
        }

        #endregion;

        public static MovementPath testPath;
        public void prepEnemies()
        {
            // Ragefaces
            //GameEntity foreverAlone = new GameEntity("ForeverAlone", true);

            this.levelEntryPoint.EntitySpawned = updateWaveGUI;
            //create the spawner for rage face one with an initial spawn delay of one second
            this.levelEntryPoint.DefaultFaction = enemyFaction;
            //we're telling it to make two faces, but they'll show up in exactly the same place right now...
            this.levelEntryPoint.AddGameEntityBatch("ForeverAlone", 10, 2, this.enemyAI);
            this.levelEntryPoint.AddGameEntityBatch("RageOkay", 15, 2, this.enemyAI);
            //rageSpawner.AddGameEntityBatch(rageOne, 2, 2);
            //rageSpawner.AddEntityBatch(rageOne, 1, 2);
            Stage.CurrentStage.AddGameEntity(this.levelEntryPoint);
            this.levelEntryPoint.Activate();

            //testPath = Pathfinder.ResolvePath(this.levelEntryPoint.GetPosition(), this.levelExitPoint.GetPosition());
            // test setting the destination ...
            //this.enemyAI.Destination = levelExitPoint.GetPosition();
            enemyAI.SetMovementPath(Pathfinder.ResolvePath(this.levelEntryPoint.GetPosition(), this.levelExitPoint.GetPosition()));
        }

        private void setPlayerCurrency(int newValue)
        {
            playerCurrency = newValue;

            GuiLayer.GetLayerByName("HUD").GetElementAt(0, 0).As<LabelValue>().SetValue(playerCurrency.ToString());
        }

        private void changePlayerCurrency(int changeAmount)
        {
            playerCurrency += changeAmount;

            GuiLayer.GetLayerByName("HUD").GetGUIElement(0).As<LabelValue>().SetValue(playerCurrency.ToString());
        }

        private void exitReached(ArtificialIntelligence ai)
        {
            ai.GetBody().Destroy();

            playerHealth -= 1;
            GuiLayer.GetLayerByName("HUD").GetGUIElement(1).As<LabelValue>().SetValue(playerHealth.ToString());

            if (playerHealth == 0)
            {
                levelDefeat();
            }
            // If there are no more enemies...
            else if (this.levelEntryPoint.Active == false && enemyFaction.EntityCount == 0)
            {
                levelVictory();
            }
        }

        // The player has won the level
        private void levelVictory()
        {
            // Show the victory splash
            new Splash(Sprite.GetSpriteByName("Success"));

            // Load the next level
        }

        // The player has lost the level
        private void levelDefeat()
        {
            // Show the defeat splash
            new Splash(Sprite.GetSpriteByName("Lose1"));

            // Reload the level
        }

        // What to do when the level has ended, regardless of whether the player has won.
        private void levelEnded()
        {
            // Change to the next level for now.
        }

    }
}