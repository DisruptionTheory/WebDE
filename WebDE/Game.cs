using System;
using System.Collections.Generic;

using SharpKit.Html;
using SharpKit.JavaScript;

using WebDE.Clock;
using WebDE.GameObjects;
using WebDE.GUI;
using WebDE.InputManager;
using WebDE.Rendering;
using WebDE.Animation;
using WebDE.Networking;
using WebDE.Audio;

namespace WebDE
{
    [JsType(JsMode.Clr, Filename = "scripts/Main.js")]
    public static class Game
    {
        public static Stage CurrentStage { get { return Stage.CurrentStage; } }
        public static IClock Clock { get; set; }
        public static ISoundSystem SoundSystem { get; set; }
        public static IRenderEngine Renderer
        {
            get { return renderer; }
            set
            {
                // Clear existing render operations.
                if (Game.clockRenderguy != null)
                {
                    Game.Clock.RemoveRender(Game.clockRenderguy);
                }
                renderer = value;
                Game.clockRenderguy = Game.Clock.AddRender(Game.Renderer.Render);
        }}

        public static bool Paused = true;

        //all of the levels in the game
        private static List<Stage> gameStages = new List<Stage>();
        private static IClientNetworkAdapter networkAdapter;
        private static LivingGameEntity playerCharacter;
        private static IRenderEngine renderer;

        //the game attributes. Should probably switch these to enums
        //environment: used for rendering and input. Defaults to HTML
        private static string environment = "HTML";

        //the unordered list of "global" enities, spanning and / or not specific to individual stages
        private static List<GameEntity> globalEntities = new List<GameEntity>();
        private static Dictionary<string, GameEntity> hashedGlobalEntities = new Dictionary<string, GameEntity>();

        //used to notify the player of various events...
        private static GuiLayer notificationLayer;
        private static string notificationHideRenderId;

        // Gotta nix this ...
        public static string clockRenderguy;

        public static void Init()
        {
            Game.Clock = new DOM_Clock();
            //Game.Clock.Start();

            //I must be script#'ing wrong. This won't fire here.
            InputManager.Input.Init();
            // Set up the local player
            Player LocalPlayer = new Player();
            //Surface.Initialize(new DOM_Renderer());
            //Surface.Initialize(new Canvas_Renderer());
            Game.Renderer = new Canvas_Renderer();
            SoundSystem = new HTML5_Audio();
            //set up the primary view
            // Remember to initialize the renderer first!
            View playerOneView = new View(LightingStyle.Tiles);
            playerOneView.SetArea(new Rectangle(0, 0, Renderer.GetSize().width, Renderer.GetSize().height));

            //notificationLayer = new GuiLayer(playerOneView, new Rectangle(0, 0, 0, 0));
            notificationLayer = playerOneView.AddLayer("NotificationLayer", new Rectangle(0, 0, 0, 0));
            notificationLayer.SetSize(522, 60);
            //notificationLayer.Hide();

            //create two gui elements on the layer:
            //an image on the left
            GuiElement notifIcon = notificationLayer.AddGUIElement("");
            notifIcon.AddStyle("notifIcon");
            notifIcon.SetPosition(12, 0);
            //and sender on the right
            GuiElement notifSender = notificationLayer.AddGUIElement("");
            notifSender.SetPosition(58, 0);
            notifSender.SetSize(474, 12);
            //and the sender's handle, if applicable
            GuiElement notifSenderHandle = notificationLayer.AddGUIElement("");
            notifSenderHandle.SetPosition(80, 0);
            notifSenderHandle.SetSize(474, 12);
            notifSenderHandle.AddStyle("tweetName");
            //and the text on the right
            GuiElement notifText = notificationLayer.AddGUIElement("");
            notifText.SetPosition(58, 18);
            notifText.SetSize(474, 40);
            notificationLayer.Hide();

            //clockRenderguy = Game.Clock.AddRender(Game.Renderer.RenderUpdate);
            //clockRenderguy = Game.Clock.AddRender(Game.Renderer.Render);

            //reposition the notification layer after a few seconds
            string notif_repos = Clock.AddRender(reposition_notification_layeer);
            Clock.delayRender(notif_repos, 2);

            Clock.AddCalculation(StandardCalculations);
            Clock.IntervalExecute(Game.StandardIntervalCalculations, .2);
            Game.Paused = false;
        }

        private static void checkResourcesLoaded()
        {
        }

        private static void afterResourcesLoaded()
        {
        }

        // For headless servers and whatnot.
        public static void MinimalInitialize()
        {
            Game.Clock = new MultiThreaded_Clock();
            Game.Clock.Start();

            Surface.Initialize(new Null_Renderer());
            //set up the primary view
            View playerOneView = new View(LightingStyle.Tiles);
            Game.Clock.AddCalculation(StandardCalculations);
            Game.Clock.IntervalExecute(Game.StandardIntervalCalculations, .2);
        }

        public static void StandardCalculations()
        {
            if (Stage.CurrentStage != null)
            {
                Stage.CurrentStage.CalculateEntities();

                if (Stage.CurrentStage.GetLights(View.GetMainView()).Count > 0)
                {
                    Stage.CurrentStage.CalculateLights(View.GetMainView());
                }
            }

            //dunno what to do about intervals...
        }

        public static void StandardIntervalCalculations()
        {
            Game.Paused = !Helpah.WindowActive();

            if (Game.Paused)
            {
                return;
            }

            if (Stage.CurrentStage != null)
            {
                Stage.CurrentStage.CalculateGameEntityPhysics();
            }
        }

        private static void reposition_notification_layeer()
        {
            Rectangle viewArea = View.GetMainView().GetArea();
            double newNotifX = viewArea.x + viewArea.width - Game.notificationLayer.GetSize().width - 12;
            double newNotifY = viewArea.x + viewArea.height - Game.notificationLayer.GetSize().height - 12;
            Game.notificationLayer.SetPosition(newNotifX, newNotifY);
        }

        /// <summary>
        /// Show a given message, with a given icon, for the given duration.
        /// </summary>
        /// <param name="icon">The sprite to display in the notification area. </param>
        /// <param name="sender">The sender's name.</param>
        /// <param name="senderHandle">The sender's handle, codename, second name, etc.</param>
        /// <param name="message">The message to display.</param>
        /// <param name="duration">How long to show the message before hiding.</param>
        public static void Notification(Sprite icon, string sender, string senderHandle, string message, int duration)
        {
            //there should be two gui elements: an icon on the left, and a text area on the right
            GuiElement notifIcon = Game.notificationLayer.GetGuiElements()[0];
            GuiElement notifSender = Game.notificationLayer.GetGuiElements()[1];
            GuiElement notifSenderHandle = Game.notificationLayer.GetGuiElements()[2];            
            GuiElement notifText = Game.notificationLayer.GetGuiElements()[3];

            notifIcon.SetSprite(icon);
            notifIcon.GetSprite().Size = new Dimension(40, 40);
            notifIcon.GetSprite().Animate();
            notifSender.SetText(sender);
            notifSenderHandle.SetText(senderHandle);
            notifText.SetText(message);

            Debug.log("Setting sender to " + sender + " and handle to " + senderHandle);

            //reposition the handle to be after the sender's name
            notifSenderHandle.SetPosition(Helpah.d2i(notifSender.GetPosition().x + (notifSender.GetText().Length * 14)), Helpah.d2i(notifSenderHandle.GetPosition().y));

            //Game.notificationLayer.Render();
            Game.notificationLayer.Show();

            //timeout hide
            notificationHideRenderId = Clock.AddRender(NotificationEnd);
            Clock.delayRender(notificationHideRenderId, duration);
        }

        public static void NotificationEnd()
        {
            Debug.log("Attempting to hide notification.");
            Game.notificationLayer.Hide();
            Clock.RemoveRender(notificationHideRenderId);
        }

        //depending on available stage types, maybe this function should infer stagetype from game context?
        public static Stage CreateStage(string stageName, StageType stageType, Dimension stageSize = null)
        {
            Stage newStage;
            if (stageSize != null)
            {
                newStage = new Stage(stageName, stageType, stageSize);
            }
            else
            {
                newStage = new Stage(stageName, stageType);
            }

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
            if (Stage.CurrentStage != null)
            {
                Game.UnloadStage();
            }

            //load the new stage
            Stage.CurrentStage = stageToLoad;
            View.GetMainView().AttachStage(stageToLoad);
        }

        public static void UnloadStage()
        {
            Renderer.ClearGameBoard();

            Stage.CurrentStage = null;
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

        public static IClientNetworkAdapter NetworkAdapter
        {
            get
            {
                if (networkAdapter == null)
                {
                    networkAdapter = new WebSocketClient();
                }
                return networkAdapter;
            }
        }

        public static LivingGameEntity GetPlayerCharacter()
        {
            return playerCharacter;
        }

        public static void SetPlayerCharacter(LivingGameEntity newCharacter)
        {
            playerCharacter = newCharacter;
        }
    }
}