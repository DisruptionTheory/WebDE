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
        private static Dictionary<UITKComponent, UITKComponent> children = new Dictionary<UITKComponent, UITKComponent>();
        private static Dictionary<string, UITKComponent> registry = new Dictionary<string, UITKComponent>();

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
            J(surface).width("100%");
            J(surface).height("100%");
            document.body.appendChild(surface);
            initialized = true;

            //capture events
            surface.onclick += clickHandler;
            surface.onmousemove += mouseHoverHandler;
            surface.onkeydown += keyDownHandler;
            surface.onkeyup += keyUpHandler;
        }

        internal static void clickHandler(HtmlDomEventArgs e)
        {
            string id = e.srcElement.id;
            if (registry.ContainsKey(id)) registry[id].FireClicked(e);
        }

        internal static void mouseHoverHandler(HtmlDomEventArgs e)
        {
            string id = e.srcElement.id;
            if (registry.ContainsKey(id)) registry[id].FireMouseHover(e);
        }

        internal static void keyDownHandler(HtmlDomEventArgs e)
        {
            string id = e.srcElement.id;
            if (registry.ContainsKey(id)) ((UITKKeyedComponent)registry[id]).FireKeyDown(e);
        }

        internal static void keyUpHandler(HtmlDomEventArgs e)
        {
            string id = e.srcElement.id;
            if (registry.ContainsKey(id)) ((UITKKeyedComponent)registry[id]).FireKeyUp(e);
        }


        /// <summary>
        /// Calls a redraw and refresh on all components;
        /// </summary>
        public static void Validate()
        {
            if (!initialized) Initialize();
            foreach(UITKComponent child in children.Values) child.Validate();
        }

        /// <summary>
        /// Redraws the component.
        /// </summary>
        /// <param name="component">The component to redraw.</param>
        internal static void Redraw(UITKComponent component)
        {
            string id = component.Id;
            HtmlElement element = document.getElementById(id);
            if (element != null)
            {
                element.outerHTML = component.Markup.GetMarkup();
                foreach (var style in component.Styles.GetStyleDictionary())
                {
                    J("#" + id).css(style.Key.ToString(), style.Value.ToString());
                }
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
        public static void AddComponent(UITKComponent component, UITKPosition position)
        {
            if (!initialized) Initialize();
            children.Add(component, component);
            component.Position = position;
            HtmlElement element = document.createElement("div");
            element.id = component.Id;
            surface.appendChild(element);
            component.Validate();
        }

        internal static string GetValue(string id)
        {
            return document.getElementById(id).As<HtmlInputText>().value;
        }

        internal static void SetValue(string id, string value)
        {
            HtmlInputText input = document.getElementById(id).As<HtmlInputText>();
            input.value = value;
        }

        internal static void RegisterId(string id, UITKComponent component)
        {
            if (registry.ContainsKey(id)) registry[id] = component;
            else registry.Add(id, component);
        }
    }
}