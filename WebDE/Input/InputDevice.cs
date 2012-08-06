using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

//input devices need "context"
//for example, some games have controls while in a vehicle, as opposed to on foot
//The same button can be bound to two different functions, depending on the game context

using WebDE;
using WebDE.GUI;

namespace WebDE.InputManager
{
    [JsType(JsMode.Clr, Filename = "../scripts/WebDE.Input.js")]
    public partial class InputDevice
    {
        public static InputDevice Mouse;
        public static InputDevice Keyboard;

        public static void InitializeDevices()
        {
            InputDevice.Mouse = new InputDevice("mouse");
            InputDevice.Keyboard = new InputDevice("keyboard");

            InputDevice.Mouse.SetButtonName(1, "mouse0");
            //InputDevice.Mouse.SetButtonName(1, "mouse1");

            //maybe use getkeycode or getkeychar javascript functions for the keyboard (for now)?
            InputDevice.Keyboard.SetButtonName(187, "Plus");
            InputDevice.Keyboard.SetButtonName(107, "NumPlus");
        }
        
        /*
        public static Point GetCursorPos()
        {
            return null;
        }
        */

        private string deviceName = "";
        //a list of all button names indexed by the button ids that the buttons are identified with
        private Dictionary<int, string> buttonNames = new Dictionary<int, string>();
        //a list of all mapped functions for this input device
        //private Dictionary<int, GUIFunction> buttonFunctions = new Dictionary<int, GUIFunction>();
        private Dictionary<int, Dictionary<ButtonCommand,GUIFunction>> buttonFunctions = new Dictionary<int, Dictionary<ButtonCommand,GUIFunction>>();
        private Dictionary<int, double> axisPositions = new Dictionary<int, double>();

        public InputDevice(string name)
        {
            this.deviceName = name;
        }

        public void SetButtonName(int buttonId, string buttonName)
        {
            this.buttonNames[buttonId] = buttonName;
        }

        public string GetButtonName(int buttonId)
        {
            return this.buttonNames[buttonId];
        }

        public int GetButtonId(string buttonName)
        {
            foreach (int buttonKey in buttonNames.Keys)
            {
                if (buttonNames[buttonKey] == buttonName)
                {
                    return buttonKey;
                }
            }

            return -1;
        }

        /// <summary>
        /// Bind a button to a function. If the buttonName is null or empty, the function will identify the button with the id.
        /// </summary>
        /// <param name="buttonName"></param>
        /// <param name="buttonId"></param>
        /// <param name="buttonFunction"></param>
        public void Bind(string buttonName, int buttonId, ButtonCommand buttonCommand, GUIFunction buttonFunction)
        {
            if (buttonName != "" && buttonName != null)
            {
                buttonId = this.GetButtonId(buttonName);
            }

            if(this.buttonFunctions[buttonId] == null)
            {
                this.buttonFunctions[buttonId] = new Dictionary<ButtonCommand,GUIFunction>();
            }
            this.buttonFunctions[buttonId][buttonCommand] = buttonFunction;
            //this.buttonFunctions[buttonId] = buttonFunction;
        }

        public void Bind(string buttonName, int buttonId, GUIFunction buttonFunction)
        {
            this.Bind(buttonName, buttonId, ButtonCommand.Down, buttonFunction);
        }

        public void UnBind(string buttonName, int buttonId)
        {
        }

        /// <summary>
        /// Get the GUIFunction mapped to the given button. If buttonName is empty or null, buttonId is used.
        /// </summary>
        /// <param name="buttonName">The name of the button.</param>
        /// <param name="buttonId">The integer id of the button.</param>
        /// <returns></returns>
        public GUIFunction GetFunctionFromButton(string buttonName, int buttonId, ButtonCommand buttonCommand)
        {
            if (buttonName != "" && buttonName != null)
            {
                buttonId = this.GetButtonId(buttonName);
            }

            if(this.buttonFunctions[buttonId] == null)
            {
                return null;
            }

            return this.buttonFunctions[buttonId][buttonCommand];
        }

        public GUIFunction GetFunctionFromButton(string buttonName, int buttonId)
        {
            return GetFunctionFromButton(buttonName, buttonId, ButtonCommand.Down);
        }

        public double GetAxisPosition(int axis)
        {
            return this.axisPositions[axis];
        }

        public void SetAxisPosition(int axis, double position)
        {
            this.axisPositions[axis] = position;
        }
    }
}
