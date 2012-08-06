using SharpKit.JavaScript;
using SharpKit.Html4;
using SharpKit.jQuery;
using WebDE.Net;

namespace MapEditor
{
    [JsType(JsMode.Clr, Filename = "scripts/MapEditor.js")]
    public class MapEditor : jQueryContextBase
    {
        static void DefaultClient_Load()
        {
            J(document.body).append("Ready<br/>");
        }
    }
}