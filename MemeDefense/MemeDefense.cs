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
    public class MemeDefense
    {
        //a placeholder for which tower we intend to place based on the clicked gui element
        private LivingGameEntity towerToPlace = null;

        public MemeDefense()
        {
            Main.Initialize();

            prepSprites();
            prepTowers();

            //load the level...
            CreateTestArea();

            //create the GUI menu for tower building
            createTowerMenu();
        }

        #region levels

        public Stage CreateTestArea()
        {
            Stage testArea = Game.CreateStage("Test Area", StageType.Tile);
            testArea.CreateRandomTiles();
            foreach (GameEntity tile in testArea.GetVisibleTiles(View.GetMainView()))
            {
                tile.SetSprite(Sprite.GetSpriteByName("Grass"));
            }

            prepEnemies(testArea);

            //Debug.CreateManualClock();
            //Debug.AddCalculation(Stage.CurrentStage.CalculateEntities);
            //Debug.AddCalculation(Stage.CurrentStage.CalculateEntityPhysics);

            //Clock.AddCalculation(Stage.CurrentStage.CalculateEntities);
            //Clock.IntervalExecute(Stage.CurrentStage.CalculateGameEntityPhysics, .2);

            return testArea;
        }

        public Stage CreatePastryLevel()
        {
            Stage pastryLevel = Game.CreateStage("FrostedPastries", StageType.Tile);
            pastryLevel.SetSize(16, 14);
            Sprite cherryPastry = Sprite.GetSpriteByName("CherryPastry");

            //cookie sheet background image (sprite)

            //create the enemy nodepath before creating the tiles
            //MovementPath enemyPath = MovementPath.ResolvePath(


            //randomize placement and icon for pastries
            for (int h = 0; h < pastryLevel.GetSize().height; h++)
            {
                for (int w = 0; w < pastryLevel.GetSize().width; w++)
                {
                    //with the randomization of the tiles, we're going to randomize whether or not this is buildiable
                    bool buildable = false;
                    int rand = JsMath.round(JsMath.random());
                    if (rand == 1)
                    {
                        buildable = true;
                    }
                    Tile aTile = pastryLevel.AddTile("", true, buildable);
                    aTile.SetParentStage(pastryLevel);
                    aTile.SetPosition(w, h);

                    if (buildable == false)
                    {
                        aTile.SetSprite(cherryPastry);
                    }
                }
            }

            return pastryLevel;
        }

        public Stage CreateAquaLevel()
        {
            //all tiles are water

            //then ship tiles on top

            Stage aquaLevel = Game.CreateStage("AquaticAdventure", StageType.Tile);

            aquaLevel.SetSize(16, 15);
            Sprite water = Sprite.GetSpriteByName("Water");
            Sprite planks = Sprite.GetSpriteByName("Planks");

            Tile waterTile = new Tile("Water", false, false);
            Tile plankTile = new Tile("Planks", true, true);

            for (int h = 0; h < aquaLevel.GetSize().height; h++)
            {
                for (int w = 0; w < aquaLevel.GetSize().width; w++)
                {
                    bool buildable = false;
                    int rand = JsMath.round(JsMath.random());
                    if (rand == 1)
                    {
                        buildable = true;
                    }
                    //Tile aTile = aquaLevel.AddTile("", true, buildable);
                    Tile aTile;

                    if (buildable == true)
                    {
                        aTile = Tile.GetByName("Planks");
                        aTile.SetSprite(planks);
                    }
                    else
                    {
                        aTile = Tile.GetByName("Water");
                        aTile.SetSprite(water);
                    }

                    aTile.SetParentStage(aquaLevel);
                    aTile.SetPosition(w, h);
                    aquaLevel.AppendTile(aTile);
                }
            }

            //set up spawn points

            return aquaLevel;
        }

        public Stage CreateSpaceLevel()
        {
            Stage spaceLevel = Game.CreateStage("Space", StageType.Tile);
            spaceLevel.SetSize(16, 14);

            //space background image (sprite)

            //random planets
            //Sprite saturn = Sprite.GetSpriteByName("Saturn");
            Tile saturnTile = new Tile("Saturn", false, false);
            //saturnTile.SetSprite(saturn);
            //random asteroid fields
            //Sprite asteroidField = Sprite.GetSpriteByName("AsteroidField");
            Tile asteroidTile = new Tile("AsteroidField", false, false);
            //asteroidTile.SetSprite(asteroidField);
            //random red dwarf stars
            //Sprite redDwarf = Sprite.GetSpriteByName("RedDwarf");
            Tile redDwarfTile = new Tile("RedDwarf", false, false);
            //redDwarfTile.SetSprite(redDwarf);

            //randomize placement and icon for pastries
            for (int h = 0; h < spaceLevel.GetSize().height; h++)
            {
                for (int w = 0; w < spaceLevel.GetSize().width; w++)
                {
                    Tile spaceTile;

                    //pick a random number based on how many different tile types are in the level
                    //(three plus "no tile" makes four)
                    int randomTileType = Helpah.Rand(1, 4);

                    //1 will represent "no tile"
                    if (randomTileType != 1)
                    {   //default to "Saturn" (let's say 2)
                        spaceTile = Tile.GetByName("Saturn");

                        if (randomTileType == 3)
                        {   //asteroid field at 3
                            spaceTile = Tile.GetByName("AsteroidField");
                        }
                        else if (randomTileType == 4)
                        {   //red dwarf at 4
                            spaceTile = Tile.GetByName("RedDwarf");
                        }

                        spaceTile.SetParentStage(spaceLevel);
                        spaceTile.SetPosition(w, h);
                        spaceTile.SetSprite(Sprite.GetSpriteByName(spaceTile.GetName()));
                        spaceLevel.AppendTile(spaceTile);
                    }

                }
            }

            //set up spawn points

            return spaceLevel;
        }

        #endregion

        public void prepTowers()
        {
            LivingGameEntity GigaPuddiTower = new LivingGameEntity("GigaPuddiTower", 100);
            LivingGameEntity NyanTower = new LivingGameEntity("NyanTower", 100);
            LivingGameEntity StaredadTower = new LivingGameEntity("StaredadTower", 100);
            LivingGameEntity CeilingCatTower = new LivingGameEntity("CeilingCatTower", 100);
            LivingGameEntity PepperCopTower = new LivingGameEntity("PepperCopTower", 100);
            LivingGameEntity XTheYTower = new LivingGameEntity("XTheYTower", 100);
            LivingGameEntity TableFlipperTower = new LivingGameEntity("TableFlipperTower", 100);

            /*
            Projectile dummyProjectile = new Projectile("dummy");
            dummyProjectile.SetAcceleration(.1);
            Weapon dummyWeapon = new Weapon(GigaPuddiTower, 10, 1000, .5, 180, 40);
            dummyWeapon.SetRange(10);
            dummyWeapon.SetProjectile(dummyProjectile);

            GigaPuddiTower.SetSprite(Sprite.GetSpriteByName("GigaPuddiTower"));
            GigaPuddiTower.AddWeapon(dummyWeapon);
            */
        }

        #region sprites;

        public static void prepSprites()
        {
            PrepPastryLevelSprites();
            PrepSpaceLevelSprites();
            PrepAquaLevelSprites();

            AnimationFrame grassFrame = new AnimationFrame("images/tiles/grass.png", 0, 0);
            Animation grassIcon = new Animation();
            grassIcon.SetName("GrassIcon");
            grassIcon.AddFrame(grassFrame);
            Sprite Grass = new Sprite("Grass");
            Grass.addAnimation(grassIcon);

            AnimationFrame frmRageOne = new AnimationFrame("images/ragefaces/7.png", 0, 0);
            Animation animRageOne = new Animation();
            animRageOne.AddFrame(frmRageOne);
            Sprite sprRageOne = new Sprite("RageOne");
            sprRageOne.addAnimation(animRageOne);

            AnimationFrame arrowFrame = new AnimationFrame("images/arrow.png", 0, 0);
            Animation ArrowAnim = new Animation();
            ArrowAnim.SetName("Bullet");
            ArrowAnim.AddFrame(arrowFrame);
            Sprite Bullet = new Sprite("Bullet");
            Bullet.addAnimation(ArrowAnim);
            Bullet.setSize(10, 14);

            AnimationFrame gigaFrame = new AnimationFrame("images/tiles/gigapuddi.png", 0, 0);
            Animation GigaPudiIcon = new Animation();
            GigaPudiIcon.SetName("GigaPudiIcon");
            GigaPudiIcon.AddFrame(gigaFrame);
            Sprite GigaTower = new Sprite("GigaPuddiTower");
            GigaTower.addAnimation(GigaPudiIcon);

            AnimationFrame nyamFrame = new AnimationFrame("images/tiles/nyancat.png", 0, 0);
            Animation NyanIcon = new Animation();
            NyanIcon.SetName("NyanIcon");
            NyanIcon.AddFrame(nyamFrame);
            Sprite NyanTower = new Sprite("NyanTower");
            NyanTower.addAnimation(NyanIcon);

            AnimationFrame CeilingCatFrame = new AnimationFrame("images/tiles/ceilingcat.png", 0, 0);
            Animation CeilingCatIcon = new Animation();
            CeilingCatIcon.SetName("CeilingCatIcon");
            CeilingCatIcon.AddFrame(CeilingCatFrame);
            Sprite CeilingCatTower = new Sprite("CeilingCatTower");
            CeilingCatTower.addAnimation(CeilingCatIcon);

            AnimationFrame StareDadFrame = new AnimationFrame("images/tiles/staredad.png", 0, 0);
            Animation StareDadIcon = new Animation();
            StareDadIcon.SetName("GigaPudiIcon");
            StareDadIcon.AddFrame(StareDadFrame);
            Sprite StareDadTower = new Sprite("StareDadTower");
            StareDadTower.addAnimation(StareDadIcon);

            AnimationFrame PepperCopFrame = new AnimationFrame("images/tiles/pepperspray.png", 0, 0);
            Animation PepperCopIcon = new Animation();
            PepperCopIcon.SetName("PepperCopIcon");
            PepperCopIcon.AddFrame(PepperCopFrame);
            Sprite PepperCopTower = new Sprite("PepperCopTower");
            PepperCopTower.addAnimation(PepperCopIcon);

            AnimationFrame XTheYFrame = new AnimationFrame("images/tiles/xthey.png", 0, 0);
            Animation XTheYIcon = new Animation();
            XTheYIcon.SetName("XTheYIcon");
            XTheYIcon.AddFrame(XTheYFrame);
            Sprite XTheYTower = new Sprite("XTheYTower");
            XTheYTower.addAnimation(XTheYIcon);

            AnimationFrame TableFlipperFrame = new AnimationFrame("images/tiles/tableflipper.png", 0, 0);
            Animation TableFlipperIcon = new Animation();
            TableFlipperIcon.SetName("TableFlipperIcon");
            TableFlipperIcon.AddFrame(TableFlipperFrame);
            Sprite TableFlipperTower = new Sprite("TableFlipperTower");
            TableFlipperTower.addAnimation(TableFlipperIcon);

            AnimationFrame GggFrame = new AnimationFrame("images/ragefaces/goodguygreg.jpg", 0, 0);
            Animation GggIcon = new Animation();
            GggIcon.SetName("GoodGuyGregIcon");
            GggIcon.AddFrame(GggFrame);
            Sprite GoodGuyGreg = new Sprite("GoodGuyGreg");
            GoodGuyGreg.addAnimation(GggIcon);

            AnimationFrame ScumbagFrame = new AnimationFrame("images/scumbagsteve.jpg", 0, 0);
            Animation ScumbagIcon = new Animation();
            ScumbagIcon.SetName("ScumbagSteveIocn");
            ScumbagIcon.AddFrame(ScumbagFrame);
            Sprite ScumbagSteve = new Sprite("ScumbagSteve");
            ScumbagSteve.addAnimation(ScumbagIcon);
        }

        public static void PrepPastryLevelSprites()
        {
            //AnimationFrame CherryFrame = new AnimationFrame("sample_levels/images/pastry/cherry.jpg", 0, 0);
            AnimationFrame CherryFrame = new AnimationFrame("images/tiles/cherry.png", 0, 0);
            Animation CherryIcon = new Animation();
            CherryIcon.SetName("CherryPastryIcon");
            CherryIcon.AddFrame(CherryFrame);
            Sprite CherryPastry = new Sprite("CherryPastry");
            CherryPastry.addAnimation(CherryIcon);
        }

        public static void PrepSpaceLevelSprites()
        {
            AnimationFrame SaturnFrame = new AnimationFrame("images/tiles/saturn.png", 0, 0);
            Animation SaturnIcon = new Animation();
            SaturnIcon.SetName("SaturnIcon");
            SaturnIcon.AddFrame(SaturnFrame);
            Sprite Saturn = new Sprite("Saturn");
            Saturn.addAnimation(SaturnIcon);

            AnimationFrame AsteroidFrame = new AnimationFrame("images/tiles/asteroid-field.png", 0, 0);
            Animation AsteroidFieldIcon = new Animation();
            AsteroidFieldIcon.SetName("AsteroidFieldIcon");
            AsteroidFieldIcon.AddFrame(AsteroidFrame);
            Sprite AsteroidField = new Sprite("AsteroidField");
            AsteroidField.addAnimation(AsteroidFieldIcon);

            AnimationFrame RedDwarfFrame = new AnimationFrame("images/tiles/red-dwarf.png", 0, 0);
            Animation RedDwarfIcon = new Animation();
            RedDwarfIcon.SetName("RedDwarfIcon");
            RedDwarfIcon.AddFrame(RedDwarfFrame);
            Sprite RedDwarf = new Sprite("RedDwarf");
            RedDwarf.addAnimation(RedDwarfIcon);
        }

        public static void PrepAquaLevelSprites()
        {
            AnimationFrame WaterFrame = new AnimationFrame("images/tiles/water.png", 0, 0);
            Animation WaterIcon = new Animation();
            WaterIcon.SetName("WaterIcon");
            WaterIcon.AddFrame(WaterFrame);
            Sprite Water = new Sprite("Water");
            Water.addAnimation(WaterIcon);

            AnimationFrame PlanksFrame = new AnimationFrame("images/tiles/planks.png", 0, 0);
            Animation PlanksIcon = new Animation();
            PlanksIcon.SetName("PlanksIcon");
            PlanksIcon.AddFrame(PlanksFrame);
            Sprite Planks = new Sprite("Planks");
            Planks.addAnimation(PlanksIcon);
        }

        #endregion;

        #region gui;

        //create the GUI menu for building towers
        public void createTowerMenu()
        {
            //create the gui functions
            GUIFunction gfAction = new GUIFunction("Action", InputDevice.Mouse, "mouse0");
            GUIFunction gfContext = new GUIFunction("Context", InputDevice.Mouse, "mouse1");
            //InputDevice.Keyboard.Bind("enter", 0, gfAction);

            //create the layer itself
            //GuiLayer towerMenu = new GuiLayer("TowerMenu", View.GetMainView(), new Rectangle(-100, 40, 146, 300));
            GuiLayer towerMenu = View.GetMainView().AddLayer("TowerMenu", new Rectangle(-100, 40, 146, 300));
            //make this the active UI layer
            towerMenu.Activate();

            GuiElement label = towerMenu.AddGUIElement("Towers");
            label.SetPosition(0, 0);
            //create a GUI element to place a tower
            GuiElement gigaTower = towerMenu.AddGUIElement("Giga Puddi Tower");
            //give it a single frame sprite for the tower icon
            gigaTower.SetSprite(Sprite.GetSpriteByName("GigaPuddiTower"));
            gigaTower.SetPosition(20, 32);
            //call the animation call once to get it to render
            gigaTower.GetSprite().Animate();
            gigaTower.SetCustomValue("GigaPuddiTower");
            //repeat for the other towers
            GuiElement nyanTower = towerMenu.AddGUIElement("Nyan Nest");
            nyanTower.SetSprite(Sprite.GetSpriteByName("NyanTower"));
            nyanTower.SetPosition(70, 32);
            nyanTower.GetSprite().Animate();
            nyanTower.SetCustomValue("NyanTower");
            GuiElement staredadTower = towerMenu.AddGUIElement("Staredad Seat");
            staredadTower.SetSprite(Sprite.GetSpriteByName("StareDadTower"));
            staredadTower.SetPosition(20, 82);
            staredadTower.GetSprite().Animate();
            staredadTower.SetCustomValue("StaredadTower");
            GuiElement ceilingCatTower = towerMenu.AddGUIElement("Celing Cat Hole");
            ceilingCatTower.SetSprite(Sprite.GetSpriteByName("CeilingCatTower"));
            ceilingCatTower.SetPosition(70, 82);
            ceilingCatTower.GetSprite().Animate();
            ceilingCatTower.SetCustomValue("CeilingCatTower");
            GuiElement pepperCopTower = towerMenu.AddGUIElement("Pepper Cop Tower");
            pepperCopTower.SetSprite(Sprite.GetSpriteByName("PepperCopTower"));
            pepperCopTower.SetPosition(20, 132);
            pepperCopTower.GetSprite().Animate();
            pepperCopTower.SetCustomValue("PepperCopTower");
            GuiElement xTheYTower = towerMenu.AddGUIElement("All The Things");
            xTheYTower.SetSprite(Sprite.GetSpriteByName("XTheYTower"));
            xTheYTower.SetPosition(70, 132);
            xTheYTower.GetSprite().Animate();
            xTheYTower.SetCustomValue("XTheYTower");
            GuiElement tableFlipperTower = towerMenu.AddGUIElement("Table Flipper");
            tableFlipperTower.SetSprite(Sprite.GetSpriteByName("TableFlipperTower"));
            tableFlipperTower.SetPosition(20, 182);
            tableFlipperTower.GetSprite().Animate();
            tableFlipperTower.SetCustomValue("TableFlipperTower");

            //set its click function to just create a tower (for now)
            //later, we'll want to have its click function put the GUI in "tower placement" mode
            //gigaTower.SetGUIFunction(gfAction, towerMenu_Click);
            foreach (GuiElement towerIcon in towerMenu.GetGuiElements())
            {
                towerIcon.SetGUIFunction(gfAction, towerMenu_Click);
            }
        }

        private string desiredTowerName;

        public void towerMenu_Click(GuiEvent gEvent)
        {
            //set up the tower that we intend to click
            //based on the custom value set in the gui element
            desiredTowerName = gEvent.clickedElement.GetCustomValue();
            if (desiredTowerName == "" || desiredTowerName == null)
            {
                //Debug.log(gEvent.clickedElement.GetId() + " has no custom value :(");
                return;
            }
            //towerToPlace = Tower.GetByName(desiredTowerName);
            towerToPlace = LivingGameEntity.CloneByName(desiredTowerName);
            //Debug.log("Desired tower name is " + desiredTowerName);


            //attach a function to the main view when the user clicks on it
            GUIFunction gfAction = GUIFunction.GetByName("Action");
            Stage.CurrentStage.GetCollisionMap().SetGUIFunction(gfAction, placeATower);
            /*
            foreach (GuiElement colTile in Stage.CurrentStage.GetCollisionMap().GetGuiElements())
            {
                colTile.SetGUIFunction(gfAction, placeATower);
            }
            */

            //show the collision map on the main playable area
            Stage.CurrentStage.ShowCollisionMap();

            //set the gui element that led us here to be highlighted
            gEvent.clickedElement.Select(true);

            //set the tower menu as the default gui layer, so that if something tries to switch to default in the future, it switches to this
            //deactivate the tower menu gui layer

            //create a gui element and set it to following the mouse

            //throw up a notification for the player
            Sprite ggg = Sprite.GetSpriteByName("GoodGuyGreg");
            Game.Notification(ggg, "Good Guy Greg", "@ggg", "Click where you would like the tower to be placed.", 10);
        }

        //test function to make a new tower...
        public void placeATower(GuiEvent gEvent)
        {
            //hide the collision map gui layer
            Stage.CurrentStage.HideCollisionMap();

            Tile clickedTile = gEvent.clickedTiles[0];
            if (clickedTile == null || desiredTowerName == "")
            {
                return;
            }

            LivingGameEntity newTower = new LivingGameEntity(desiredTowerName, 100);

            newTower.SetParentStage(Stage.CurrentStage);

            //Tower newTower = Tower.GetByName("GigaPuddiTower");
            newTower.SetPosition(clickedTile.GetPosition().x, clickedTile.GetPosition().y);
            //dummyProjectile.SetAcceleration(.1);
            Weapon dummyWeapon = new Weapon(newTower, 10, 1, .5, 180, 40);
            dummyWeapon.SetRange(50);
            //dummyWeapon.SetProjectile(dummyProjectile);
            newTower.AddWeapon(dummyWeapon);

            Stage.CurrentStage.AddLivingGameEntity(newTower);

            Stage.CurrentStage.HideCollisionMap();

            //deselect the selected tower in the gui menu
            GuiLayer towerLayer = GuiLayer.GetLayerByName("TowerMenu");
            //towerLayer.SelectItem("By putting this text here, everything will be deselected, but nothing will be selected.", -1);
        }

        #endregion;

        public void prepEnemies(Stage currentStage)
        {
            //face 1 entity
            Sprite sprRageOne = Sprite.GetSpriteByName("RageOne");
            LivingGameEntity rageOne = new LivingGameEntity("RageOne", 100);
            rageOne.SetSprite(sprRageOne);
            rageOne.SetAcceleration(.1);

            //face 1 ai
            MovementPath ragePath = MovementPath.ResolvePath(
                new Point(6, 0),
                new Point(8, 6));
            ArtificialIntelligence aiRage = new ArtificialIntelligence();
            aiRage.SetMovementPath(ragePath);
            rageOne.SetAI(aiRage);

            //create the spawner for rage face one with an initial spawn delay of one second
            GameEntitySpawner rageSpawner = new GameEntitySpawner("RageSpawner", 1);
            //we're telling it to make two faces, but they'll show up in exactly the same place right now...
            //////////////////////ALL OF THESE SPAWN AT ONCE AND IT IS AN ISSUE THAT IS GOING TO NEED TO BE ADDRESSED.../////////////////////
            rageSpawner.AddGameEntityBatch(rageOne, 1, 2);
            //rageSpawner.AddEntityBatch(rageOne, 1, 2);
            rageSpawner.SetPosition(6, 0);
            currentStage.AddGameEntity(rageSpawner);
            rageSpawner.Activate();

            //no vegeta for now...
            /*
            //so the need for the negatives in the x values is still a little wierd, and I have to figure out what it is that's making it do that
            AnimationFrame frameOne = new AnimationFrame("images/vegeta.gif", -378, 380);
            AnimationFrame frameTwo = new AnimationFrame("images/vegeta.gif", -420, 384);
            Animation vegieStand = new Animation();
            vegieStand.AddFrame(frameOne);
            vegieStand.AddFrame(frameTwo);
            Sprite sprVegeta = new Sprite("Vegeta");
            sprVegeta.addAnimation(vegieStand);

            LivingEntity lenTestEnemy = new LivingEntity("Vegeta", 100);
            lenTestEnemy.SetSprite(sprVegeta);
            lenTestEnemy.GetSprite().setSize(32, 90);
            currentStage.AddEntity(lenTestEnemy);

            MovementPath vegetaPath = MovementPath.ResolvePath(
                new Point(2, 0),
                new Point(2, 4));
            //this doesn't seem to work right...
            /*
            MovementPath vegetaPath = new MovementPath(new List<Point>(
                new Point(2, 0),
                new Point(2, 4),
                new Point(6, 4)));
            * /
            ArtificialIntelligence aiVegeta = new ArtificialIntelligence();
            aiVegeta.SetMovementPath(vegetaPath);
            lenTestEnemy.SetAI(aiVegeta);
            //put the test entity at the start of the path
            lenTestEnemy.SetPosition(vegetaPath.GetNode(0).x, vegetaPath.GetNode(0).y);
            lenTestEnemy.SetAcceleration(.1);
            */
        }
    }
}