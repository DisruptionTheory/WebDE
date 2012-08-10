using System;
using System.Collections.Generic;
using SharpKit.JavaScript;
using SharpKit.Html4;
using SharpKit.jQuery;

namespace UITK
{
    [JsType(JsMode.Clr, Filename = "scripts/UITK.js")]
    public class Surface : jQueryContextBase
    {
        /// <summary>
        /// The width of the viewable inner area of the browser window.
        /// </summary>
        public static int Width
        {
            get { return window.innerWidth; }
        }

        /// <summary>
        /// The height of the viewable inner area of the broswer window.
        /// </summary>
        public static int Height
        {
            get { return window.innerHeight; }
        }
    }
}