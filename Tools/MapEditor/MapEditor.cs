using SharpKit.JavaScript;
using SharpKit.Html4;
using SharpKit.jQuery;
using WebDE.Net;

namespace MapEditor
{
    [JsType(JsMode.Clr, Filename = "scripts/MapEditor.js")]
    public class MapEditor : jQueryContextBase
    {
        public static GameClient Server
        {
            get;
            private set;
        }

        public static void Initialize()
        {
            Server = new GameClient("localhost", 81);
            Views.LoadView(Views.Login);
        }
    }
}