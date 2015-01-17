using System;
using System.Collections.Generic;

using SharpKit.JavaScript;
using SharpKit.Html4;
using SharpKit.jQuery;

using WebDE.GameObjects;
using WebDE.GUI;
using WebDE.Rendering;

namespace WebDE.InputManager
{
    [JsType(JsMode.Clr, Filename = "../scripts/WebDE.Input.js")]
    public partial class Input
    {
        private static List<JsNumber> heldKeys = new List<JsNumber>();
        private static bool windowHasFocus = true;
        private static short scrollSpeed = 10;

        //bind the mouse and keyboard according to the dom way of doing things
        public static void Bind_Input_DOM()
        {
            Debug.log("Binding input to dom");
            //J(document).bind("mousedown", jQuery_handleClick);
            new jQuery(HtmlContext.document).bind("mousedown", jQuery_handleClick);
            new jQuery(HtmlContext.document).bind("mouseup", jQuery_handleMouseUp);
            new jQuery(HtmlContext.document).bind("keydown", jQuery_handleKeyboardDown);
            new jQuery(HtmlContext.document).bind("keyup", jQuery_handleKeyboardUp);
            new jQuery(HtmlContext.document).bind("mousemove", jQuery_handleMouseMove);

            try
            {
                HtmlContext.document.attachEvent("mousewheel", new HtmlDomEventHandler(dom_handleMouseWheel));
            }
            catch (Exception ex)
            {
                Debug.log("Mousewheel broke. Blame someone.");
            }

            //jQuery.Document.Bind("click", new jQueryEventHandler(handlejQueryClick));
            //jQuery.Document.Bind("keydown", new jQueryEventHandler(handlejQueryKeyboard));
            
            //Game.Clock.AddCalculation(Input.CheckMousePos);
        }

        //public static void handlejQueryClick(jQueryEvent jqevt)
        public static void jQuery_handleClick(Event eventData)
        {
            //get the position of the renderable  / clickable area offset from the screen
            //this is useful with centered dom elements, letterboxing, etc.
            //jQueryObject gameArea = jQuery.FromElement(Document.GetElementById("gameWrapper"));
            
            //get the position of the click within the game area by subtracting the position of the render area from the click position
            int clickX = eventData.clientX - Helpah.Parse(HtmlContext.document.getElementById("gameWrapper").style.left);
            int clickY = eventData.clientY - Helpah.Parse(HtmlContext.document.getElementById("gameWrapper").style.top);

            Input.ProcessMouseButtonEvent(eventData.which, clickX, clickY, ButtonCommand.Down);
        }

        public static void jQuery_handleMouseUp(Event eventData)
        {
            //get the position of the renderable  / clickable area offset from the screen
            //this is useful with centered dom elements, letterboxing, etc.
            //jQueryObject gameArea = jQuery.FromElement(Document.GetElementById("gameWrapper"));

            //get the position of the click within the game area by subtracting the position of the render area from the click position
            int clickX = eventData.clientX - Helpah.Parse(HtmlContext.document.getElementById("gameWrapper").style.left);
            int clickY = eventData.clientY - Helpah.Parse(HtmlContext.document.getElementById("gameWrapper").style.top);

            Input.ProcessMouseButtonEvent(eventData.which, clickX, clickY, ButtonCommand.Up);
        }

        public static void jQuery_handleKeyboardDown(Event eventData)
        {
            // Do some fenagling so that this only registers when the button is depressed, and not held.
            if (!heldKeys.Contains(eventData.keyCode))
            {
                heldKeys.Add(eventData.keyCode);
                Input.ProcessKeyboardEvent(eventData.which, ButtonCommand.Down);
            }
        }

        public static void jQuery_handleKeyboardUp(Event eventData)
        {
            heldKeys.Remove(eventData.keyCode);
            Input.ProcessKeyboardEvent(eventData.which, ButtonCommand.Up);
        }

        private static bool checkingMousePos = false;

        public static void jQuery_handleMouseMove(Event eventData)
        {
            InputDevice.Mouse.SetAxisPosition(0, eventData.clientX);
            InputDevice.Mouse.SetAxisPosition(1, eventData.clientY);

            /*
            // Whatever. We'll just do this for now.
            if (!checkingMousePos)
            {
                Game.Clock.AddCalculation(Input.CheckMousePos);
                checkingMousePos = true;
            }
            */
        }

        public static void dom_handleMouseWheel(HtmlDomEventArgs eventData)
        {
            if (eventData.wheelDelta > 0)
            {
                Input.ProcessMouseButtonEvent("wheel", (int)InputDevice.Mouse.GetAxisPosition(0), 
                    (int)InputDevice.Mouse.GetAxisPosition(1), ButtonCommand.Up);
            }
            else
            {
                Input.ProcessMouseButtonEvent("wheel", (int)InputDevice.Mouse.GetAxisPosition(0), 
                    (int)InputDevice.Mouse.GetAxisPosition(1), ButtonCommand.Down);
            }
        }

        // Check the position of the mouse and act according to input settings.
        public static void CheckMousePos()
        {
            // If the window isn't focused, do nothing.
            if (!windowHasFocus) return;

            // If the mouse is within limits of the bounds, move the view in that direction ... 
            short bufferSize = 50;
            //View affectedView = View.GetMainView();

            foreach (View affectedView in View.GetActiveViews())
            {
                // If the mouse's x axis is on the left side of the screen ...
                if (InputDevice.Mouse.GetAxisPosition(0) < bufferSize)
                {
                    // Scrolling right
                    affectedView.SetOffsets(
                        (int)affectedView.OffsetX - scrollSpeed, (int)affectedView.OffsetY);
                }
                else if (InputDevice.Mouse.GetAxisPosition(0) > Game.Renderer.GetSize().width - bufferSize)
                {
                    // Scrolling left
                    affectedView.SetOffsets(
                        (int)affectedView.OffsetX + scrollSpeed, (int)affectedView.OffsetY);
                }

                if (InputDevice.Mouse.GetAxisPosition(1) < bufferSize)
                {
                    // Scrolling right
                    affectedView.SetOffsets(
                        (int)affectedView.OffsetX, (int)affectedView.OffsetY - scrollSpeed);
                }
                else if (InputDevice.Mouse.GetAxisPosition(1) > Game.Renderer.GetSize().height - bufferSize)
                {
                    // Scrolling left
                    affectedView.SetOffsets(
                        (int)affectedView.OffsetX, (int)affectedView.OffsetY + scrollSpeed);
                }
            }
        }
    }
}