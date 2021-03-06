﻿//using System;
//using System.Collections.Generic;

//using SharpKit.JavaScript;
//using SharpKit.Html4;
//using SharpKit.jQuery;

//namespace WebDE.Input
//{
//    [JsType(JsMode.Clr, Filename = "../scripts/WebDE.Input.js")]
//    public partial class Input
//    {
//        //whether or not to disable mouseover events, etc.
//        //primarily used to make touchscreen / mobile run smoother
//        private static bool disableAdvancedMouseInput = false;
//        //whether we're using onscreen keyboard keys or default keyboard input
//        private static bool onscreenKeyboard = false;

//        //list of local keychar values and their associated engine functions
//        //example: "w" = "Move Forward"

//        public static void Init()
//        {
//            //bind the default keyboard keys from the config file

//            //bind the mouse and keyboardfunctions
//            Bind_Input_DOM();
//            InputDevice.InitializeDevices();

//            //load the user's saved config settings, if they exist
//        }

//        /// <summary>
//        /// Load an indexed set of config settings
//        /// </summary>
//        /// <param name="configKeyVals"></param>
//        public static void LoadConfig(Dictionary<string, string> configKeyVals)
//        {
//        }

//        public static void ProcessMouseButtonEvent(int buttonId, int clickX, int clickY)
//        {
//            //get the event that the left click button is bound to
//            //if null, get the default
//            InputFunction buttonFunction = InputDevice.Mouse.GetFunctionFromButton("", buttonId);
//            //so the click gets shifted, but the GUI isn't in the area of the play...

//            //loop through all of the active gui layers
//            foreach (GuiLayer activeLayer in GuiLayer.GetActiveLayers())
//            {
//                //skip this layer if the layer doesn't exist in the clicked space
//                if (clickX > activeLayer.GetArea().Right() || clickX < activeLayer.GetArea().x || clickY > activeLayer.GetArea().Bottom() || clickY < activeLayer.GetArea().y)
//                {
//                    continue;
//                }
//                //translate the coordinates to be relative to the gui layer?
//                Point actionLocation = new Point(clickX, clickY);
//                //fire a new gui event on the layer...
//                //tell the GUI layer which action was triggered and (if applicable) where
//                activeLayer.GUI_Event(buttonFunction, actionLocation);
//            }

//            /*
//            this.viewyGUI.Click(clickEvent);

//            GuiLayer activeLayer = GuiLayer.GetActiveLayer();
//            GuiElement clickedElement = activeLayer.GetElementAt(jquibbles.ClientX, jquibbles.ClientY);
//            //now get the element in the active activeLayer at the click location
//            if (clickedElement != null)
//            {
//                clickedElement.DoAction(jquibbles);
//            }
//             */
//        }

//        public static void ProcessKeyboardEvent(int buttonId)
//        {
//            InputFunction buttonFunction = InputDevice.Keyboard.GetFunctionFromButton("", buttonId);

//            //loop through all of the active gui layers
//            foreach (GuiLayer activeLayer in GuiLayer.GetActiveLayers())
//            {
//                //translate the coordinates to be relative to the gui layer?
//                Point actionLocation = new Point(0, 0);
//                //fire a new gui event on the layer...
//                //tell the GUI layer which action was triggered and (if applicable) where
//                activeLayer.GUI_Event(buttonFunction, actionLocation);
//            }
//        }
//    }
//}
