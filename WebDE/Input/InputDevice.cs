using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

//input devices need "context"
//for example, some games have controls while in a vehicle, as opposed to on foot
//The same button can be bound to two different functions, depending on the game context

using WebDE;
using WebDE.GUI;
using WebDE.GameObjects;

namespace WebDE.InputManager
{
    [JsType(JsMode.Clr, Filename = "../scripts/WebDE.Input.js")]
    public partial class InputDevice
    {

        #region Static variables and methods

        public static InputDevice Mouse;
        public static InputDevice Keyboard;

        public static void InitializeDevices()
        {
            InputDevice.Mouse = new InputDevice("mouse", isCursor: true);
            InputDevice.Keyboard = new InputDevice("keyboard");

            InputDevice.Mouse.SetButtonName(1, "mouse0");
            //InputDevice.Mouse.SetButtonName(1, "mouse1");

            // Set up basic values for mouse axes
            // X
            InputDevice.Mouse.SetAxisPosition(0, 0);
            // Y
            InputDevice.Mouse.SetAxisPosition(1, 0);

            //maybe use getkeycode or getkeychar javascript functions for the keyboard (for now)?
            InputDevice.Keyboard.SetButtonName(32, "space");
            InputDevice.Keyboard.SetButtonName(65, "a");
            InputDevice.Keyboard.SetButtonName(68, "d");
            InputDevice.Keyboard.SetButtonName(83, "s");
            InputDevice.Keyboard.SetButtonName(87, "w");
            InputDevice.Keyboard.SetButtonName(187, "Plus");
            InputDevice.Keyboard.SetButtonName(107, "NumPlus");
        }

        /*
        public static Point GetCursorPos()
        {
            return null;
        }
        */
#endregion

        private string deviceName = "";
        //a list of all button names indexed by the button ids that the buttons are identified with
        private Dictionary<int, string> buttonNames = new Dictionary<int, string>();
        //a list of all mapped functions for this input device
        //private Dictionary<int, GUIFunction> buttonFunctions = new Dictionary<int, GUIFunction>();
        private Dictionary<int, Dictionary<ButtonCommand,GUIFunction>> buttonFunctions = new Dictionary<int, Dictionary<ButtonCommand,GUIFunction>>();
        private Dictionary<int, double> axisPositions = new Dictionary<int, double>();
        
        // Pointing device only: Edge scrolling
        private bool edgeScrollingEnabled = true;
        public bool EdgeScrollingEnabled { get { return this.edgeScrollingEnabled; } }

        /// <summary>
        /// Whether or not the input device is a cursor.
        /// </summary>
        public readonly bool IsCursor = false;

        public InputDevice(string name, bool isCursor = false)
        {
            this.deviceName = name;
            this.IsCursor = isCursor;
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

            // This will call (manual) javascript that will (attempt to) convert the character into a keycode.
            return Helpah.CharCodeFromChar(buttonName);
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

            if(!this.buttonFunctions.ContainsKey(buttonId))
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
            Debug.log("Trying to unbind " + buttonName + " but its not implemented!");
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

            if(!this.buttonFunctions.ContainsKey(buttonId))
            {
                return null;
            }

            if (this.buttonFunctions[buttonId].ContainsKey(buttonCommand))
            {
                return this.buttonFunctions[buttonId][buttonCommand];
            }

            return null;
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

        // When the mouse touches the edge of the screen, scroll.
        // Only do this when the window is active / has focus ... 
        public void EnableEdgeScrolling()
        {
            this.edgeScrollingEnabled = true;
        }

        public void DisableEdgeScrolling()
        {
            this.edgeScrollingEnabled = false;
        }

        public Point GetPosition()
        {
            Point returnPoint = null;

            if (this.axisPositions.Count > 0)
            {
                returnPoint = new Point(this.axisPositions[0], 0);

                if (this.axisPositions.Count > 1)
                {
                    returnPoint.y = this.axisPositions[1];
                }
            }

            return returnPoint;
        }
    }
}
