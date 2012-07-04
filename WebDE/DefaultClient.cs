using SharpKit.JavaScript;
using SharpKit.jQuery;
using SharpKit.Html4;

using System.Linq;
using System.Collections.Generic;

using WebDE.Timekeeper;
using WebDE.GameObjects;
using WebDE.Rendering;
using WebDE.Animation;

namespace WebDE
{
    [JsType(JsMode.Clr, Filename = "scripts/Main.js")]
    //[JsType(JsMode.Clr, Filename = "res/Default.js")]
    public class DefaultClient : jQueryContextBase
    {
        private static jQueryContextBase mainBase;

        public static void Initialize()
        {
            //(new Main()).DefaultClient_Load();

            //need to initialize a jquerycontextbase and set it to the static variable, which this will do
            new DefaultClient();

            Game.Init();

            Clock.Start();
        }

        public DefaultClient()
        {
            mainBase = this;
        }

        public static HtmlDocument GetDocument()
        {
            return document;
        }

        public static HtmlWindow GetWindow()
        {
            return window;
        }

        public static jQueryContextBase GetContext()
        {
            //return this;
            return DefaultClient.mainBase;
        }

        public static void consolelog(string message)
        {
            console.log(message);
        }
    }

    /*
    [JsType(JsMode.Global, Filename = "res/Default.js")]
    public static class Main
    {
        public static void Initialize()
        {
            //console.log("I do be initializin");
        }
    }
    */
}