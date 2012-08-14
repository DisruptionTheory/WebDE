using System;
using System.Collections.Generic;
using SharpKit.JavaScript;
using SharpKit.Html4;
using SharpKit.jQuery;

namespace UITK
{
    [JsType(JsMode.Clr, Filename = "scripts/UITK.js")]
    public class ImageBox : UITKComponent
    {
        private string src = string.Empty;
        public string Source 
        {
            get { return src; }
            set { src = value; Redraw(); }
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
        /// Recreate the internal markup and css for this component.
        /// </summary>
        public override void RecreateHTML()
        {
            string markup = "<div ";
            markup += "id='" + Id + "' ";
            markup += "></div>";
            Markup.SetMarkup(markup);

            Styles.SetRule("background-image", "url('" + src + "')");
            Styles.SetRule("left", Position.X);
            Styles.SetRule("right", Position.Y);
            Styles.SetHeight(Height);
            Styles.SetWidth(Width);
        }





        
    }
}