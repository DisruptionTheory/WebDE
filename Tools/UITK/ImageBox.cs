using System;
using System.Collections.Generic;
using SharpKit.JavaScript;
using SharpKit.Html4;
using SharpKit.jQuery;

namespace UITK
{
    [JsType(JsMode.Clr, Filename = "scripts/UITK.js")]
    public class ImageBox : UITKComponentBase
    {
        private string src = string.Empty;
        public string Source 
        {
            get { return src; }
            set { src = value; }
        }
        /// <summary>
        /// Create a new ImageBox.
        /// </summary>
        /// <param name="height">The ImageBox Height.</param>
        /// <param name="width">The ImageBox Width.</param>
        /// <param name="source">The image source url.</param>
        public ImageBox(int height, int width, string source)
        {
            Height = height;
            Width = width;
            src = source;
        }

        /// <summary>
        /// Redraw and refresh the component.
        /// </summary>
        public override void Validate()
        {
            recreateHTML();
            Surface.Redraw(this);
        }

        /// <summary>
        /// Recreate the internal markup and css for this component.
        /// </summary>
        private void recreateHTML()
        {
            string markup = "<img ";
            markup += "src='" + Source + "' ";
            markup += "id='" + Id + "' ";
            markup += "width='" + Width + "' ";
            markup += "height='" + Height + "' ";
            markup += "/>";
            Markup.SetMarkup(markup);

            Styles.SetRule("left", Position.X.ToString());
            Styles.SetRule("right", Position.Y.ToString());
        }





        
    }
}