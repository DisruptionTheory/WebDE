using System;
using System.Collections.Generic;

using SharpKit.JavaScript;
using SharpKit.Html;
using SharpKit.jQuery;

namespace UITK
{
    [JsType(JsMode.Clr, Filename = "scripts/UITK.js")]
    public abstract class UITKComponent
    {

        public UITKComponent()
        {
            Surface.RegisterId(Id, this);
            Position = new UITKPosition(0, 0, this);
            Styles = new UITKStyles(this);
            Markup = new UITKMarkup("", this);
            Id = Surface.GenerateID();
            Height = 0;
            Width = 0;
        }
        /// <summary>
        /// Forces a redraw and refresh on the component.
        /// </summary>
        public void Validate()
        {
            RecreateHTML();
            Redraw();
        }

        /// <summary>
        /// Recreate the objects html.
        /// </summary>
        public abstract void RecreateHTML();

        /// <summary>
        /// The image box height;
        /// </summary>
        public int Height
        {
            get;
            internal set;
        }

        /// <summary>
        /// The image box width.
        /// </summary>
        public int Width
        {
            get;
            internal set;
        }

        /// <summary>
        /// THe position of the Imagebox.
        /// </summary>
        public UITKPosition Position
        {
            get;
            internal set;
        }

        /// <summary>
        /// The css rules associated with the ImageBox.
        /// </summary>
        public UITKStyles Styles
        {
            get;
            internal set;
        }

        /// <summary>
        /// The HTML markup generated for the ImageBox.
        /// </summary>
        public UITKMarkup Markup
        {
            get;
            internal set;
        }

        /// <summary>
        /// The DOM id of the component.
        /// </summary>
        public string Id
        {
            get;
            private set;
        }

        internal void Redraw()
        {
            Surface.Redraw(this);
        }

        

        #region Events
        public event ClickEventHandler Click;
        public event MouseHoverEventHandler MouseHover;

        internal void FireClicked(Event args)
        {
            if(Click != null) Click(this, new UITKMouseEventArguments(this, args.pageX, args.pageY, args.ctrlKey, args.shiftKey, args.altKey, args.keyCode));
        }

        internal void FireMouseHover(Event args)
        {
            if (MouseHover != null) MouseHover(this, new UITKMouseEventArguments(this, args.pageX, args.pageY, args.ctrlKey, args.shiftKey, args.altKey, args.keyCode));
        }
        #endregion
    }
}