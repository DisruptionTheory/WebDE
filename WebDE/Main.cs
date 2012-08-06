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
    public class Main : jQueryContextBase
    {
        private static jQueryContextBase mainBase;

        public static void Initialize()
        {
            //(new Main()).DefaultClient_Load();

            //need to initialize a jquerycontextbase and set it to the static variable, which this will do
            new Main();

            Game.Init();

            Clock.Start();
        }

        public Main()
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
            return Main.mainBase;
        }

        public static void consolelog(string message)
        {
            console.log(message);
        }
    }
}