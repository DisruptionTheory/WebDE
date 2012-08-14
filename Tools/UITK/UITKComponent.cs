using System;
using System.Collections.Generic;
using SharpKit.JavaScript;
using SharpKit.Html4;
using SharpKit.jQuery;

namespace UITK
{
    [JsType(JsMode.Clr, Filename = "scripts/UITK.js")]
    public abstract class UITKComponent
    {
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
            get { return internalHeight; }
            internal set { internalHeight = value; }
        }
        private int internalHeight;

        /// <summary>
        /// The image box width.
        /// </summary>
        public int Width
        {
            get { return internalWidth; }
            internal set { internalWidth = value; }
        }
        private int internalWidth;

        /// <summary>
        /// THe position of the Imagebox.
        /// </summary>
        public UITKPosition Position
        {
            get { return internalPosition; }
            internal set { internalPosition = value; }
        }
        private UITKPosition internalPosition = new UITKPosition(0, 0);

        /// <summary>
        /// The css rules associated with the ImageBox.
        /// </summary>
        public UITKStyles Styles
        {
            get { return internalStyles; }
            internal set { internalStyles = value; }
        }
        private UITKStyles internalStyles = new UITKStyles();

        /// <summary>
        /// The HTML markup generated for the ImageBox.
        /// </summary>
        public UITKMarkup Markup
        {
            get { return internalMarkup; }
            internal set { internalMarkup = value; }
        }
        private UITKMarkup internalMarkup = new UITKMarkup("");

        /// <summary>
        /// The DOM id of the component.
        /// </summary>
        public string Id
        {
            get 
            {
                if (internalId != string.Empty) return internalId;
                internalId = Surface.GenerateID();
                return internalId;
            }
        }
        private string internalId = string.Empty;

        internal void Redraw()
        {
            Surface.Redraw(this);
        }
    }
}