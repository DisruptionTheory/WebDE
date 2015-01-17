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
    public class Debug
    {
        private static List<String> debugLog = new List<string>();
        private static List<Action> manualClockRenders = new List<Action>();
        private static List<Action> manualClockCalculations = new List<Action>();
        private static bool manualClockCreated = false;
        // Whether or not to render debug watches.
        public static bool showDebug = true;

        // For tracking variables per second (such as frames)
        private bool trackingPerSec = false;
        // Number of updates.
        private int updateCount = 0;
        // Time elapsed
        private DateTime lastLogged = DateTime.MinValue;
        // List of previous values.
        private List<int> previousValues = new List<int>();
        // Calculated average.
        private double psAvg = 0;

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

        /// <summary>
        /// Log a message to the debug log.
        /// Will eventually either go to file or console depending on project settings.
        /// Also need to add timestamp based on project settings.
        /// And it would be nice to have that "collapse similar" thing Unity does.
        /// </summary>
        /// <param name="message">The message to enter.</param>
        /// <param name="warning">Whether or not the message is a warning (warnings are ignored when debug messages are off)</param>
        public static void log(string message, bool warning = false)
        {
            if (showDebug == false && warning == true) return;

            debugLog.Add(message);
            //message = message.Replace("'", "\\'");
            //apostrophes cause infinite loops. not sure why yet. circumventing for now. need to fix later
            message = message.Replace("'", "");

            HtmlContext.console.log(message);
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
        private DateTime lastUpdated = DateTime.MinValue;

        public Debug()
        {
        }

        private static GuiLayer GetDebugLayer()
        {
            if (DebugLayer == null)
            {
                Debug.DebugLayer = View.GetMainView().AddLayer("DebugLayer", new Rectangle(0, -40, 400, 40));
                Debug.DebugLayer.RenderTarget = Render_Target.Div;
            }

            return Debug.DebugLayer;
        }

        //a value we'd like to watch...
        public static Debug Watch(string label, string value, bool trackPerSecond = false)
        {
            if (showDebug == false) return null;

            if (Debug.DebugLayer == null)
            {
                Debug.Render();
            }

            Debug returnDebug = null;

            foreach (Debug watch in Debug.Watches)
            {
                if (watch.label == label)
                {
                    if (value == watch.value)
                    {
                        return watch;
                    }

                    returnDebug = watch;
                    break;
                }
            }

            if (returnDebug == null)
            {
                returnDebug = new Debug();
                returnDebug.debugElement = Debug.GetDebugLayer().AddGUIElement("Debug element.");
                returnDebug.debugElement.SetPosition(0, Debug.DebugLayer.GetGuiElements().Count * 20);
                Debug.Watches.Add(returnDebug);
                returnDebug.lastLogged = DateTime.Now;
            }

            if (trackPerSecond)
            {
                // Increment the number of times the variable has been updated (this second)
                returnDebug.updateCount++;
                // If a second or more has passed
                if (DateTime.Now.Subtract(returnDebug.lastLogged).Seconds >= 1)
                {
                    returnDebug.previousValues.Add(returnDebug.updateCount);
                    returnDebug.updateCount = 0;
                    returnDebug.psAvg = 0;

                    for (int i = 0; i < returnDebug.previousValues.Count; i++)
                    {
                        returnDebug.psAvg += returnDebug.previousValues[i];
                    }
                    returnDebug.psAvg = Helpah.Round(returnDebug.psAvg / returnDebug.previousValues.Count, 2);
                }
            }

            returnDebug.label = label;
            returnDebug.value = value;
            returnDebug.lastUpdated = DateTime.Now;
            returnDebug.trackingPerSec = trackPerSecond;

            //returnDebug.debugElement.SetCSSPosition();
            Debug.Render();

            return returnDebug;
        }

        public void UpdateValue(string newValue)
        {
            if (this.value != newValue)
            {
                this.value = newValue;
                lastUpdated = DateTime.Now;
                Debug.Render();
            }
        }

        public static void Render()
        {
            if (Debug.showDebug == false)
            {
                return;
            }

            foreach (Debug watch in Debug.Watches)
            {
                if (watch.trackingPerSec)
                {
                    /*
                    string textStr = "<a title=\"Last updated: " + watch.lastUpdated.Minute + " : " + watch.lastUpdated.Second + "\">" +
                        watch.label + " : " + watch.value + "</a>" +
                        "<br \\>" +
                        "<a>" + watch.updateCount + " / sec. Avg : " + watch.psAvg + " / sec.</a>";
                     */
                    string textStr = watch.label + " : " + watch.value + "( " + watch.updateCount + " / sec. Avg : " + watch.psAvg + " / sec. )";

                    watch.debugElement.SetText(textStr);
                } else {
                    /*
                    watch.debugElement.SetText("<a title=\"Last updated: " + watch.lastUpdated.Minute + " : " + watch.lastUpdated.Second + "\">" + 
                        watch.label + " : " + watch.value
                        + "</a>")
                     */
                    watch.debugElement.SetText(watch.label + " : " + watch.value);
                }
            }
        }
    }
}