using System;
using System.Collections.Generic;
using SharpKit.JavaScript;
using SharpKit.Html;
using SharpKit.jQuery;

namespace UITK
{
    
    [JsType(JsMode.Clr, Filename = "scripts/UITK.js")]
    public abstract class UITKKeyedComponent : UITKComponent
    {
        #region Events

        public event KeyUpEventHandler KeyUp;
        public event KeyDownEventHandler KeyDown;

        internal void FireKeyUp(Event e)
        {
            if(KeyUp != null) KeyUp(this, new UITKKeyboardEventArguments(this, e.keyCode, e.ctrlKey, e.shiftKey, e.altKey));
        }

        internal void FireKeyDown(Event e)
        {
            if (KeyDown != null) KeyDown(this, new UITKKeyboardEventArguments(this, e.keyCode, e.ctrlKey, e.shiftKey, e.altKey));
        }
        #endregion
    }
}