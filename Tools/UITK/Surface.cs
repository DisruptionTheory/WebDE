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
        private static bool initialized = false;
        private static HtmlElement surface;
        private static Dictionary<UITKComponentBase, UITKComponentBase> children = new Dictionary<UITKComponentBase, UITKComponentBase>();
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

        private static void Initialize()
        {
            //set default css
            HtmlElement link = document.createElement("link");
            link.setAttribute("type", "text/css");
            link.setAttribute("rel", "stylesheet");
            link.setAttribute("href", "http://yui.yahooapis.com/3.6.0/build/cssreset/cssreset-min.css");
            document.getElementsByTagName("head")[0].appendChild(link);

            //create top level surface container
            surface = document.createElement("div");
            surface.id = "surface";
            J(surface).css("position", "absolute");
            J(surface).css("left", "0px");
            J(surface).css("right", "0px");
            J(surface).css("top", "0px");
            J(surface).css("bottom", "0px");
            document.body.appendChild(surface);
            initialized = true;
        }

        /// <summary>
        /// Calls a redraw and refresh on all components;
        /// </summary>
        public static void Validate()
        {
            if (!initialized) Initialize();
            foreach(UITKComponentBase child in children.Values) child.Validate();
        }

        /// <summary>
        /// Redraws the component.
        /// </summary>
        /// <param name="component">The component to redraw.</param>
        internal static void Redraw(UITKComponentBase component)
        {
            string id = component.Id;
            HtmlElement element = document.getElementById(id);
            element.outerHTML = component.Markup.GetMarkup();
            foreach (var style in component.Styles.GetStyleDictionary())
            {
                console.log(element.ToString());
                console.log("Setting style " + style.Key + " to " + style.Value + " for object " + component.Id);
                element.style[style.Key] = style.Value;
            }
        }

        /// <summary>
        /// Generate a new id for an element.
        /// </summary>
        /// <returns>A newly generated id.</returns>
        internal static string GenerateID()
        {
            string buffer = string.Empty;
            for (int i = 0; i < 16; i++)
            {
                buffer += chars[(int)(JsMath.random()*chars.Length)];
            }
            return buffer;
        }
        private static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// Add a new component to the surface at the specified x and y coordinate.
        /// </summary>
        /// <param name="component">The component to add to the surface.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public static void AddComponent(UITKComponentBase component, int x, int y)
        {
            if (!initialized) Initialize();
            children.Add(component, component);
            component.Position.X = x;
            component.Position.Y = y;
            surface.innerHTML += "<div id='" + component.Id + "'></div>";
            component.Validate();
        }
    }
}