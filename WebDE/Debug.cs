using System;
using System.Collections.Generic;

using SharpKit.JavaScript;
using SharpKit.Html;
using SharpKit.jQuery;

using WebDE.GUI;
using WebDE.Rendering;
using WebDE.InputManager;

namespace WebDE
{
    [JsType(JsMode.Clr, Filename = "scripts/Main.js")]
    public class Debug : jQueryContextBase
    {
        private static List<String> debugLog = new List<string>();
        private static List<Action> manualClockRenders = new List<Action>();
        private static List<Action> manualClockCalculations = new List<Action>();
        private static bool manualClockCreated = false;

        public static void Message(string message, string msgName)
        {
            if (msgName == "" || msgName == null)
            {
                //throw it in an unmarked area
            }
            else
            {
                //check if a market for it exists
                //if one does, update that marker (div, span, whatever)
                //otherwise, create a new marker
            }
        }

        //log a message to the debug log
        public static void log(string message)
        {
            debugLog.Add(message);
            //message = message.Replace("'", "\\'");
            //apostrophes cause infinite loops. not sure why yet. circumventing for now. need to fix later
            message = message.Replace("'", "");

            console.log(message);
        }

        //allows an action to be done on "manual" strokes of a clock
        //by default, the clock is 'advanced' by hitting the '+' key
        //if statefulness is ever implemented, it can be retreaded with the '-' key
        public static void DebugClock(GuiEvent buttonTrigger)
        {
            //if the 'clock' doesn't exist, create it...
            if (Debug.manualClockCreated == false)
            {
                CreateManualClock();
            }

            //advance the clock
            foreach (Action act in Debug.manualClockRenders)
            {
                act.Invoke();
            }
            foreach (Action act in Debug.manualClockCalculations)
            {
                act.Invoke();
            }
        }

        public static void AddCalculation(Action calculationToAdd)
        {
            Debug.manualClockCalculations.Add(calculationToAdd);
        }

        public static void AddRender(Action renderToAdd)
        {
            Debug.manualClockRenders.Add(renderToAdd);
        }

        //intervals...?

        public static void CreateManualClock()
        {
            //gui function, bound to the "plus" key (=/+)
            GUIFunction debugClock = new GUIFunction("DebugClock", InputDevice.Keyboard, "Plus");
            //bind it to the numpad as well
            InputDevice.Keyboard.Bind("NumPlus", 107, debugClock);

            //make sure that the debug layer is rendered...
            Debug.Render();

            //keybinding
            //InputManager.InputDevice.Keyboard.Bind("Plus", 0, debugClock);
            Debug.DebugLayer.SetGUIFunction(debugClock, Debug.DebugClock);

            //clock itself...

            Debug.manualClockCreated = true;
        }

        private static GuiLayer DebugLayer;
        private static List<Debug> Watches = new List<Debug>();

        private GuiElement debugElement;
        private string label;
        private string value;

        public Debug()
        {
        }

        private static GuiLayer GetDebugLayer()
        {
            if (DebugLayer == null)
            {
                Debug.DebugLayer = View.GetMainView().AddLayer("DebugLayer", new Rectangle(0, -40, 800, 40));
            }

            return Debug.DebugLayer;
        }

        //a value we'd like to watch...
        public static Debug Watch(string label, string value)
        {
            if (Debug.DebugLayer == null)
            {
                Debug.Render();
            }

            Debug returnDebug = new Debug();
            bool exists = false;

            foreach (Debug watch in Debug.Watches)
            {
                if (watch.label == label)
                {
                    returnDebug = watch;
                    exists = true;
                }
            }

            returnDebug.label = label;
            returnDebug.value = value;

            if (exists == false)
            {
                returnDebug.debugElement = Debug.GetDebugLayer().AddGUIElement("Debug element.");
                returnDebug.debugElement.SetPosition(0, Debug.DebugLayer.GetGuiElements().Count * 20);
                Debug.Watches.Add(returnDebug);
            }

            //returnDebug.debugElement.SetCSSPosition();
            Debug.Render();

            return returnDebug;
        }

        public void UpdateValue(string newValue)
        {
            this.value = newValue;
            Debug.Render();
        }

        public static void Render()
        {
            foreach (Debug watch in Debug.Watches)
            {
                watch.debugElement.SetText(watch.label + " : " + watch.value);
            }
        }
    }
}