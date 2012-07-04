using System;
using System.Collections.Generic;

using SharpKit.Html;
using SharpKit.JavaScript;

using WebDE.Timekeeper;
using WebDE.GameObjects;
using WebDE.GUI;
using WebDE.InputManager;
using WebDE.Rendering;
using WebDE.Animation;

namespace WebDE
{
    [JsType(JsMode.Clr, Filename = "scripts/Main.js")]
    public class Game
    {
        //all of the levels in the game
        private static List<Stage> gameStages = new List<Stage>();
        private static IRenderEngine renderer;

        //the game attributes. Should probably switch these to enums
        //environment: used for rendering and input. Defaults to HTML
        private static string environment = "HTML";

        //the unordered list of "global" enities, spanning and / or not specific to individual stages
        private static List<GameEntity> globalEntities = new List<GameEntity>();
        private static Dictionary<string, GameEntity> hashedGlobalEntities = new Dictionary<string, GameEntity>();

        //used to notify the player of various events...
        private static GuiLayer notificationLayer;

        public static void Init()
        {
            //I must be script#'ing wrong. This won't fire here.
            InputManager.Input.Init();
            Surface.Initialize(new DOM_Renderer());
            //set up the primary view
            View playerOneView = new View(LightingStyle.Tiles);
            playerOneView.SetArea(new Rectangle(0, 0, GetRenderer().GetSize().Item1, GetRenderer().GetSize().Item2));

            //notificationLayer = new GuiLayer(playerOneView, new Rectangle(0, 0, 0, 0));
            notificationLayer = playerOneView.AddLayer("NotificationLayer", new Rectangle(0, 0, 0, 0));
            notificationLayer.SetSize(522, 60);
            //notificationLayer.Hide();

            //create two gui elements on the layer:
            //an image on the left
            GuiElement notifIcon = notificationLayer.AddGUIElement("");
            notifIcon.SetPosition(2, 0);
            //and sender on the right
            GuiElement notifSender = notificationLayer.AddGUIElement("");
            notifSender.SetPosition(-2, 0);
            notifSender.SetSize(474, 12);
            //and the text on the right
            GuiElement notifText = notificationLayer.AddGUIElement("");
            notifText.SetPosition(-2, 18);
            notifText.SetSize(474, 40);

            //reposition the notification layer after a few seconds
            string notif_repos = Clock.AddRender(reposition_notification_layeer);
            Clock.delayRender(notif_repos, 2);

            Clock.AddCalculation(StandardCalculations);
            Clock.IntervalExecute(Game.StandardIntervalCalculations, .2);
        }

        public static void StandardCalculations()
        {
            if (Stage.CurrentStage != null)
            {
                Stage.CurrentStage.CalculateEntities();

                if (Stage.CurrentStage.GetLights().Count > 0)
                {
                    Stage.CurrentStage.CalculateLights();
                }
            }

            //dunno what to do about intervals...
        }

        public static void StandardIntervalCalculations()
        {
            Debug.log("Calculating game entity physics.");
            if (Stage.CurrentStage != null)
            {
                Stage.CurrentStage.CalculateGameEntityPhysics();
            }
        }

        private static void reposition_notification_layeer()
        {
            Rectangle viewArea = View.GetMainView().GetViewArea();
            double newNotifX = viewArea.x + viewArea.width - Game.notificationLayer.GetSize().Item1 - 12;
            double newNotifY = viewArea.x + viewArea.height - Game.notificationLayer.GetSize().Item2 - 12;
            Game.notificationLayer.SetPosition(newNotifX, newNotifY);
        }

        /// <summary>
        /// Show a given message, with a given icon, for the given duration.
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="message"></param>
        /// <param name="duration"></param>
        public static void Notification(Sprite icon, string sender, string message, int duration)
        {
            //there should be two gui elements: an icon on the left, and a text area on the right
            GuiElement notifIcon = Game.notificationLayer.GetGuiElements()[0];
            GuiElement notifSender = Game.notificationLayer.GetGuiElements()[1];
            GuiElement notifText = Game.notificationLayer.GetGuiElements()[2];

            notifIcon.SetSprite(icon);
            notifIcon.GetSprite().setSize(40, 40);
            notifIcon.GetSprite().Animate();
            notifSender.SetText(sender);
            notifText.SetText(message);

            //Game.notificationLayer.Render();
            //Game.notificationLayer.Show();

            //timeout hide
        }

        //depending on available stage types, maybe this function should infer stagetype from game context?
        public static Stage CreateStage(string stageName, StageType stageType)
        {
            Stage newStage = new Stage(stageName, stageType);

            //if there is a main view with no stage, make this the attached stage
            if (View.GetMainView() != null && View.GetMainView().GetAttachedStage() == null)
            {
                View.GetMainView().AttachStage(newStage);
            }

            gameStages.Add(newStage);

            return newStage;
        }

        public static void LoadStage(Stage stageToLoad)
        {
            //unload the current stage
            //load the new stage
        }

        //set a game attribute (currently unused)
        public static void SetAttribute(string attributeValue)
        {
            if (attributeValue.ToLower() == "html")
            {
                environment = "HTML";
            }
        }

        public static string GetEnvironment()
        {
            return environment;
        }

        public static GameEntity GetGlobalGameEntity(object GameEntityidentifier)
        {
            //entities can be identified a number of ways
            //probably, for now, just string... (name)

            //depending on the identifier given, search either the hashed or unordered first

            //if it's not found in that list, then switch to the other

            return null;
        }

        public static IRenderEngine GetRenderer()
        {
            return renderer;
        }

        public static void SetRenderer(IRenderEngine newRenderer)
        {
            renderer = newRenderer;
        }
    }
}