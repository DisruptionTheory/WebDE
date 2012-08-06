using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.GameObjects;
using WebDE.InputManager;

namespace WebDE.GUI
{
    [JsType(JsMode.Clr, Filename = "../scripts/GUI.js")]
    public partial class GUIFunction
    {
        private static List<GUIFunction> guiFunctions = new List<GUIFunction>();
        private static GUIFunction defaultFunction;

        public static GUIFunction GetByName(string functionName)
        {
            foreach (GUIFunction gf in GUIFunction.guiFunctions)
            {
                if (gf.GetName() == functionName)
                {
                    return gf;
                }
            }
            return null;
        }

        public static GUIFunction GetDefaultFunction()
        {
            return GUIFunction.defaultFunction;
        }

        public static void SetDefaultFunction(GUIFunction newFunc)
        {
            GUIFunction.defaultFunction = newFunc;
        }

        //the name of the event
        private string eventName = "";
        //how long the user must wait before (re-)using the gui function, in milliseconds
        private double firingDelay = 250;
        //the last time the event was fired
        private double lastFire = 0;
        //the list of input names that are bound to this function
        private List<string> boundButtons = new List<string>();

        /// <summary>
        /// Creates a new gui function with the given name, binds it to the named button on the given device.
        /// </summary>
        /// <param name="name">The GUI Function's name</param>
        /// <param name="bindingDevice">The Input Device to bind to.</param>
        /// <param name="defaultButtonName"></param>
        public GUIFunction(string name, InputDevice bindingDevice, string defaultButtonName, ButtonCommand buttonCommand)
        {
            this.eventName = name;
            bindingDevice.Bind(defaultButtonName, 0, buttonCommand, this);

            //if there is no default function, it's this now
            if (GUIFunction.defaultFunction == null)
            {
                GUIFunction.defaultFunction = this;
            }

            GUIFunction.guiFunctions.Add(this);
        }

        public GUIFunction(string name, InputDevice bindingDevice, string defaultButtonName)
        {
            this.eventName = name;
            bindingDevice.Bind(defaultButtonName, 0, ButtonCommand.Down, this);

            //if there is no default function, it's this now
            if (GUIFunction.defaultFunction == null)
            {
                GUIFunction.defaultFunction = this;
            }

            GUIFunction.guiFunctions.Add(this);
        }

        public string GetName()
        {
            return this.eventName;
        }
    }

    public enum ButtonCommand
    {
        Down,
        Up,
        Press,
        Hold,
        Double
    }
}
