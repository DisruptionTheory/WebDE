using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

namespace WebDE.GUI
{
    [JsType(JsMode.Clr, Filename = "../scripts/GUI.js")]
    public partial class Hint : GuiElement
    {
        //the text that will appear in the hint box
        //private string text;
        //the image that will appear to the side of the hint box
        //private Sprite sprIcon;
        //the title 
        private string title;

        private bool hasAction = false;

        //we can only show one hint at a time, so there should be like a global or static hint that gets displayed / updated
        public Hint(GuiLayer owningLayer, string elementText) :
            base(owningLayer, elementText)
        {
            //set height to 285 pixels
            //set width to "auto"? or let CSS handle that?
            //add a CSS class
            //jQueryObject thisGuy = jQuery.FromElement(this.GetRenderElement());
            //thisGuy.AddClass("GUIHint");
        }
    }
}
