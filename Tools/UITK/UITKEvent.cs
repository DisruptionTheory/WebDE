using System;
using System.Collections.Generic;
using SharpKit.JavaScript;
using SharpKit.Html;
using SharpKit.jQuery;


namespace UITK
{
    public delegate void ClickEventHandler(UITKComponent sender, UITKMouseEventArguments e);
    public delegate void MouseHoverEventHandler(UITKComponent sender, UITKMouseEventArguments e);
    public delegate void KeyDownEventHandler(UITKComponent sender, UITKKeyboardEventArguments e);
    public delegate void KeyUpEventHandler(UITKComponent sender, UITKKeyboardEventArguments e);
    public delegate void TextChangedEventHandler(UITKKeyedComponent sender);

    [JsType(JsMode.Clr, Filename = "scripts/UITK.js")]
    public class UITKEventArguments
    {
        public UITKComponent Sender
        {
            get;
            private set;
        }

        public bool CtrlKey
        {
            get;
            private set;
        }

        public bool Shiftkey
        {
            get;
            private set;
        }

        public bool Altkey
        {
            get;
            private set;
        }

        internal UITKEventArguments(UITKComponent sender, bool ctrl, bool shift, bool alt)
        {
            Sender = sender;
            CtrlKey = ctrl;
            Shiftkey = shift;
            Altkey = alt;
        }
    }

    [JsType(JsMode.Clr, Filename = "scripts/UITK.js")]
    public class UITKMouseEventArguments : UITKEventArguments
    {
        public int MouseX
        {
            get;
            private set;
        }

        public int MouseY
        {
            get;
            private set;
        }

        public int Button
        {
            get;
            private set;
        }

        internal UITKMouseEventArguments(UITKComponent sender, int x, int y, bool ctrl, bool shift, bool alt, int button)
            : base(sender, ctrl, shift, alt)
        {
            MouseX = x;
            MouseY = y;
            Button = button;
        }
    }

    [JsType(JsMode.Clr, Filename = "scripts/UITK.js")]
    public class UITKKeyboardEventArguments : UITKEventArguments
    {
        public int KeyCode
        {
            get;
            private set;
        }
        internal UITKKeyboardEventArguments(UITKComponent sender, int keyCode, bool ctrl, bool shift, bool alt)
            : base(sender, ctrl, shift, alt) 
        {
            KeyCode = keyCode;
        }
    }
}