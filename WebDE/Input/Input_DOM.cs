using System;
using System.Collections.Generic;

using SharpKit.JavaScript;
using SharpKit.Html4;
using SharpKit.jQuery;

using WebDE.GameObjects;

namespace WebDE.InputManager
{
    [JsType(JsMode.Clr, Filename = "../scripts/WebDE.Input.js")]
    public partial class Input : jQueryContextBase
    {
        //bind the mouse and keyboard according to the dom way of doing things
        public static void Bind_Input_DOM()
        {
            Debug.log("Binding input to dom");
            J(document).bind("click", handlejQueryClick);
            J(document).bind("keydown", handlejQueryKeyboard);

            //jQuery.Document.Bind("click", new jQueryEventHandler(handlejQueryClick));
            //jQuery.Document.Bind("keydown", new jQueryEventHandler(handlejQueryKeyboard));
        }

        //public static void handlejQueryClick(jQueryEvent jqevt)
        public static void handlejQueryClick(Event eventData)
        {
            //get the position of the renderable  / clickable area offset from the screen
            //this is useful with centered dom elements, letterboxing, etc.
            //jQueryObject gameArea = jQuery.FromElement(Document.GetElementById("gameWrapper"));
            
            //get the position of the click within the game area by subtracting the position of the render area from the click position
            int clickX = eventData.clientX - Helpah.Parse(document.getElementById("gameWrapper").style.left);
            //int clickX = jqevt.ClientX;
            int clickY = eventData.clientY - Helpah.Parse(document.getElementById("gameWrapper").style.top);
            //int clickY = jqevt.ClientY;

            Input.ProcessMouseButtonEvent(eventData.which, clickX, clickY);
        }

        public static void handlejQueryKeyboard(Event eventData)
        {
            Input.ProcessKeyboardEvent(eventData.which);
        }
    }
}