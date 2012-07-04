//using System;
//using System.Collections.Generic;

//using SharpKit.JavaScript;

//namespace WebDE.Input
//{
//    [JsType(JsMode.Clr, Filename = "../scripts/WebDE.Input.js")]
//    public partial class InputFunction
//    {
//        private static List<InputFunction> inputFunctions = new List<InputFunction>();
//        private static InputFunction defaultFunction;

//        public static InputFunction GetByName(string functionName)
//        {
//            foreach (InputFunction inFunc in InputFunction.inputFunctions)
//            {
//                if (inFunc.GetName() == functionName)
//                {
//                    return inFunc;
//                }
//            }
//            return null;
//        }

//        public static InputFunction GetDefaultFunction()
//        {
//            return InputFunction.defaultFunction;
//        }

//        public static void SetDefaultFunction(InputFunction newFunc)
//        {
//            InputFunction.defaultFunction = newFunc;
//        }

//        //the name of the event
//        private string eventName = "";
//        //how long the user must wait before (re-)using the gui function, in milliseconds
//        private double firingDelay = 250;
//        //the last time the event was fired
//        private double lastFire = 0;
//        //the list of input names that are bound to this function
//        private List<string> boundButtons = new List<string>();

//        /// <summary>
//        /// Creates a new gui function with the given name, binds it to the named button on the given device.
//        /// </summary>
//        /// <param name="name">The GUI Function's name</param>
//        /// <param name="bindingDevice">The Input Device to bind to.</param>
//        /// <param name="defaultButtonName"></param>
//        public InputFunction(string name, InputDevice bindingDevice, string defaultButtonName)
//        {
//            this.eventName = name;
//            bindingDevice.Bind(defaultButtonName, 0, this);

//            //if there is no default function, it's this now
//            if (InputFunction.defaultFunction == null)
//            {
//                InputFunction.defaultFunction = this;
//            }

//            InputFunction.inputFunctions.Add(this);
//        }

//        public string GetName()
//        {
//            return this.eventName;
//        }
//    }
//}
