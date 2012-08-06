using System;
using System.Collections.Generic;

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
using WebDE.Timekeeper;

namespace Lights
{
    [JsType(JsMode.Clr, Filename = "../../Lights/Lights_Compiled.js")]
    public class Main// : jQueryContextBase
    {
        public static void Initialize()
        {
            new Lights();
        }
    }

    [JsType(JsMode.Clr, Filename = "../../Lights/Lights_Compiled.js")]
    public class Lights
    {
        //a placeholder for which tower we intend to place based on the clicked gui element
        private LivingGameEntity towerToPlace = null;

        public Lights()
        {
            Main.Initialize();

            //load the level...
            Stage testArea = CreateTestArea();

            Color randomColorOne = new Color(0, 100, 0);
            Color randomColorTwo = new Color(0, 0, 100);

            //create shadowstones of composite colors (at least two composition)
            Lightstone shadowStone = createShadowStone(randomColorOne, randomColorTwo);

            //create lightstones in range that can be activated to neutralize shadowstones, give them the appropriate colors
            createLightStone(randomColorOne, shadowStone);
            createLightStone(randomColorTwo, shadowStone);

            Clock.TimedExecute(CreateTestGUI, 1);
            Clock.AddRender(checkLightstones);
        }

        public Stage CreateTestArea()
        {
            Stage testArea = Game.CreateStage("Test Area", StageType.Tile);
            testArea.SetTileSize(10, 10);
            testArea.SetSize(80, 60);
            testArea.SetBackgroundColor(Color.Black);
            testArea.CreateRandomTiles();

            /*
            for (int h = 0; h < testArea.GetSize().Item2; h++)
            {
                for (int w = 0; w < testArea.GetSize().Item1; w++)
                {
                    //with the randomization of the tiles, we're going to randomize whether or not this is buildiable
                    int rand = JsMath.round(JsMath.random());
                    //int rand = (new Random()).Next(0, 1);
                    //check whether the random # is 0 or 1, the long way, because apparently boolean.parse won't figure it out
                    bool buildable = false;
                    if (rand == 1)
                    {
                        buildable = true;
                    }
                    //Tile aTile = new Tile("", true, buildable);
                    Tile aTile = testArea.AddTile("", true, buildable);
                    aTile.SetLightLevel(Color.Black);
                    aTile.SetParentStage(testArea);
                    aTile.SetPosition(w, h);
                    //stageTiles.Add(aTile);
                }
            }
            */
            
            foreach (GameEntity tile in testArea.GetVisibleTiles(View.GetMainView()))
            {
                //tile.SetSprite(Sprite.GetSpriteByName("Grass"));
                ((Tile) tile).SetLightLevel(Color.Black);
            }
            //new GuiLayer("Steve", View.GetMainView(), 

            //GuiLayer.GetActiveLayers()[0].SetGUIFunction(gfAction, testeh);

            return testArea;
        }

        public void checkLightstones()
        {
            foreach (LightSource light in Stage.CurrentStage.GetLights())
            {
                if (light is Lightstone)
                {
                    List<LightSource> lightsInRange = Stage.CurrentStage.GetLightsNear(light.GetPosition(), 10 * Stage.CurrentStage.GetTileSize().Item1);
                    lightsInRange.Remove(light);
                    if (lightsInRange.Count > 0)
                    {
                        light.Show();
                    }
                    else
                    {
                        light.Hide();
                    }
                }
            }
        }

        #region GUI

        public void CreateTestGUI()
        {
            //GUIFunction gfAction = new GUIFunction("Action", InputDevice.Mouse, "mouse0", ButtonCommand.Press);
            GUIFunction gfMouseDown = new GUIFunction("MouseDown", InputDevice.Mouse, "mouse0", ButtonCommand.Down);
            GUIFunction gfMouseUp = new GUIFunction("MouseUp", InputDevice.Mouse, "mouse0", ButtonCommand.Up);
            GuiLayer testLayer = View.GetMainView().AddLayer("CollisionLayer", DOM_Renderer.GetRenderer().As<DOM_Renderer>().BoardArea());

            GuiLayer cursorLayer = View.GetMainView().AddLayer("CursorLayer", new Rectangle(0, 0, 100, 100));
            cursorLayer.FollowCursor(true);
            cursorLayer.Deactivate();
            //add content to the cursor layer
            GuiElement cursorColorIndicator = cursorLayer.AddGUIElement("");
            //cursorColorIndicator.AddStyle("gradientMan");
            cursorColorIndicator.SetStyle("Background", Gradient.ToString(new Color(0, 80, 200)));
            cursorColorIndicator.SetSize(100, 100);
            cursorLayer.Hide();

            testLayer.SetGUIFunction(gfMouseDown, chargeLazer);
            testLayer.SetGUIFunction(gfMouseUp, fireLazer);
        }

        private Color newLightColor;
        private string chargeInterval;

        public void chargeLazer(GuiEvent evt)
        {
            //show the color we're going to create
            GuiLayer.GetLayerByName("CursorLayer").Show();

            //start with red
            newLightColor = new Color(1, 0, 0);

            //add an interval to the clock to advance the color to display
            chargeInterval = Clock.AddRender(colorTicker);
        }

        public void fireLazer(GuiEvent evt)
        {
            //stop the periodic color growth
            Clock.RemoveRender(chargeInterval);

            //remove (hide) the gui element created in chargeLazer
            GuiLayer.GetLayerByName("CursorLayer").Hide();

            int range = 10;

            //create the light
            LightSource plCol = new LightSource(evt.eventPos.x, evt.eventPos.y, 5, range);
            plCol.SetDiminishing(true);
            plCol.SetColor(newLightColor);

            //create a shadow creature, of the same color as the player dropped

            //chase away any nearby shadow creatures colored opposite to what the player dropped

            /*
            //show / affect any nearby lightstones
            List<LightSource> lightsInRange = Stage.CurrentStage.GetLightsNear(evt.eventPos, range);
            foreach (LightSource light in lightsInRange)
            {
                if (light is Lightstone)
                {
                    light.Show();
                    Clock.TimedExecute(light.Hide, 1);
                }
            }
            */
        }

        private int currentLevel = 0;

        private void NextLevel()
        {
            //clear all lightsources and lightstones from the board
            foreach(LightSource light in Stage.CurrentStage.GetLights())
            {
                light.Destroy();
            }

            //clear all shadowcreatures from the board
            foreach (LivingGameEntity shadowCreature in Stage.CurrentStage.GetVisibleEntities(View.GetMainView()))
            {
                shadowCreature.Destroy();
            }

            currentLevel += 1;

            Stage newStage = CreateTestArea();

            for (int i = 0; i < currentLevel; i++)
            {
                Color randomColorOne = new Color(0, 100, 0);
                Color randomColorTwo = new Color(0, 0, 100);

                //create shadowstones of composite colors (at least two composition)
                Lightstone shadowStone = createShadowStone(randomColorOne, randomColorTwo);

                //create lightstones in range that can be activated to neutralize shadowstones, give them the appropriate colors
                createLightStone(randomColorOne, shadowStone);
                createLightStone(randomColorTwo, shadowStone);
            }
        }

        public bool CheckForVictory()
        {
            foreach (LightSource light in Stage.CurrentStage.GetLights())
            {
                //need to (properly) check if it's a shadow stone, rather than just checking if color is black
                if (light is Lightstone && light.GetColor() != Color.Black)
                {
                    //if the light is not 'activated' or fully lit
                    return false;
                }
            }

            NextLevel();
            return true;
        }

        public void colorTicker()
        {
            if(newLightColor.red > 0 && newLightColor.red < 255)
            {
                newLightColor.red += 1;
                if (newLightColor.blue > 0)
                {
                    newLightColor.blue -= 5;
                }
            }
            if (newLightColor.green > 0 && newLightColor.green < 255)
            {
                newLightColor.green += 1;
                if (newLightColor.red > 0)
                {
                    newLightColor.red -= 5;
                }
            }
            else
            {
                newLightColor.blue += 1;
                if (newLightColor.green > 0)
                {
                    newLightColor.green -= 5;
                }
                if (newLightColor.blue == 255)
                {
                    newLightColor.red = 1;
                }
            }
            
            GuiLayer cursorLayer = GuiLayer.GetLayerByName("CursorLayer");
            cursorLayer.GetElementAt(0, 0).SetStyle("Background", Gradient.ToString(newLightColor));
        }

        #endregion

        public Lightstone createLightStone(Color color, Lightstone nearShadow)
        {
            int stoneRange = 20;

            //pick a position within range of the shadowstone
            int stoneX = Helpah.Rand(int.Parse((nearShadow.GetPosition().x - nearShadow.GetRange()).ToString()), int.Parse((nearShadow.GetPosition().x + nearShadow.GetRange()).ToString()));
            int stoneY = Helpah.Rand(int.Parse((nearShadow.GetPosition().y - nearShadow.GetRange()).ToString()), int.Parse((nearShadow.GetPosition().y + nearShadow.GetRange()).ToString()));

            //pick a random position on the board that lets it have full range

            Lightstone newStone = new Lightstone(
                stoneX, stoneY, 10, stoneRange);
            newStone.SetColor(color);
            //newStone.AddCustomStyling("gradientMan");
            //newStone.SetPosition(10, 10);
            newStone.SetParentStage(WebDE.GameObjects.Stage.CurrentStage);
            //WebDE.GameObjects.Stage.CurrentStage.AddLivingGameEntity(newStone);
            newStone.Hide();
            //stage is 80 wide and 60 tall. therefore...

            return newStone;
        }

        public Lightstone createShadowStone(Color colOne, Color colTwo)
        {
            int stoneRange = 20;
            Color stoneColor = new Color(colOne.red + colTwo.red, colOne.green + colTwo.green, colOne.blue + colTwo.blue);

            //pick a random position on the board that lets it have full range
            int stoneX = Helpah.Rand(stoneRange, Stage.CurrentStage.GetSize().Item1 - stoneRange);
            int stoneY = Helpah.Rand(stoneRange, Stage.CurrentStage.GetSize().Item2 - stoneRange);

            Lightstone shadowStone = new Lightstone(
                stoneX, stoneY, 10, stoneRange);
            shadowStone.SetColor(stoneColor);
            shadowStone.SetParentStage(Stage.CurrentStage);
            shadowStone.AddCustomStyling("shadowStone");    //this modifies the z-index...

            return shadowStone;
        }

        public void createShadowCreature(int x, int y)
        {
            LivingGameEntity shadowCreature = new LivingGameEntity("ShadowCreature", 100);
            shadowCreature.AddCustomStyling("shadowCreature");
            shadowCreature.SetParentStage(Stage.CurrentStage);
            Stage.CurrentStage.AddLivingGameEntity(shadowCreature);
            shadowCreature.SetPosition(x, y);

            ArtificialIntelligence creatureAI = new ArtificialIntelligence();
            creatureAI.SetMovementThoughtPattern(customCreatureThoughtPattern);
            shadowCreature.SetAI(creatureAI);
        }

        private MovementDirection GetOppositeDirection(Point origin, Point antiDestination)
        {
            return MovementDirection.Down;
        }

        private void customCreatureThoughtPattern(ArtificialIntelligence creatureAI)
        {
            Point creaturePos = creatureAI.GetBody().GetPosition();
            Color creatureColor = Color.Black;
            List<LightSource> nearLights = LightSource.GetLocalLightSources(creaturePos.x, creaturePos.y);

            //if there's a light of the opposite color close to us, run from it, and don't think of anything else
            foreach (LightSource light in nearLights)
            {
                if (light is Lightstone)
                {
                    continue;
                }

                if (light.GetColor().IsOpposite(creatureColor))
                {
                    //run away!
                    creatureAI.GetBody().SetDirection(GetOppositeDirection(creaturePos, light.GetPosition()));
                }
            }

            //if there's an active lightstone nearby, check if we're in range to be able to attack it


            //if we are in range, attack it
            //if we're not in range, move closer to it
        }
    }
}