using System;
using System.Collections.Generic;
using SharpKit.JavaScript;
using SharpKit.Html;
using SharpKit.jQuery;

namespace UITK
{
    [JsType(JsMode.Clr, Filename = "scripts/UITK.js")]
    public class Surface : jQueryContext
    {
        private static bool initialized = false;
        private static Element surface;
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
            Element link = document.createElement("link");
            link.setAttribute("type", "text/css");
            link.setAttribute("rel", "stylesheet");
            link.setAttribute("href", "http://yui.yahooapis.com/3.6.0/build/cssreset/cssreset-min.css");
            document.getElementsByTagName("head")[0].appendChild(link);

            //create top level surface container
            surface = document.createElement("div");
            surface.setAttribute("id", "surface");
            J(surface).css("position", "absolute");
            J(surface).width("100%");
            J(surface).height("100%");
            document.body.appendChild(surface);
            initialized = true;

            //capture events
            window.onresize += resizeHandler;
            //surface.onclick += clickHandler;
            //surface.onmousemove += mouseHoverHandler;
            //surface.onkeydown += keyDownHandler;
            //surface.onkeyup += keyUpHandler;

            new jQuery(HtmlContext.document).bind("onclick", clickHandler);
            new jQuery(HtmlContext.document).bind("mousemove", mouseHoverHandler);
            new jQuery(HtmlContext.document).bind("onkeydown", keyDownHandler);
            new jQuery(HtmlContext.document).bind("onkeyup", keyUpHandler);
        }

        internal static void resizeHandler(DOMEvent e)
        {
            Validate();
        }

        internal static void clickHandler(Event e)
        {
            string id = e.srcElement.id;
            if (registry.ContainsKey(id)) registry[id].FireClicked(e);
        }

        internal static void mouseHoverHandler(Event e)
        {
            string id = e.srcElement.id;
            if (registry.ContainsKey(id)) registry[id].FireMouseHover(e);
        }

        internal static void keyDownHandler(Event e)
        {
            string id = e.srcElement.id;
            if (registry.ContainsKey(id)) ((UITKKeyedComponent)registry[id]).FireKeyDown(e);
        }

        internal static void keyUpHandler(Event e)
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
            HtmlElement element = (HtmlElement)document.getElementById(id);
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
        public static void AddComponent(UITKComponent component)
        {
            if (!initialized) Initialize();
            children.Add(component, component);
            Element element = document.createElement("div");
            element.setAttribute("id", component.Id);
            surface.appendChild(element);
            component.Validate();
        }

        internal static string GetValue(string id)
        {
            return document.getElementById(id).textContent;
            //return document.getElementById(id).As<HtmlInputText>().value;
        }

        internal static void SetValue(string id, string value)
        {
            document.getElementById(id).textContent = value;
            //HtmlInputText input = document.getElementById(id).As<HtmlInputText>();
            //input.value = value;
        }

        internal static void RegisterId(string id, UITKComponent component)
        {
            if (registry.ContainsKey(id)) registry[id] = component;
            else registry.Add(id, component);
        }
    }
}