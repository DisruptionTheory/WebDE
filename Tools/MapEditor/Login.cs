using SharpKit.JavaScript;
using SharpKit.Html;
using SharpKit.jQuery;
using WebDE;

namespace MapEditor
{
    [JsType(JsMode.Clr, Filename = "scripts/MapEditor.js")]
    public class Login
    {
        public static void TryLogin()
        {
            string password = jQueryContext.J("#password").text();
            string username = jQueryContext.J("#username").text();
            MapEditor.Server.GetApikey(username, password, ApiKeyCallback);
            MapEditor.Server.GetGroups(GroupsCallback);
        }

        public static void ApiKeyCallback(JsObject result)
        {
            HtmlContext.window.alert(result["apikey"].ToString());
        }

        public static void GroupsCallback(JsObject result)
        {
            HtmlContext.window.alert(result.toString());
        }
    }
}