using SharpKit.JavaScript;
using SharpKit.Html4;
using SharpKit.jQuery;
using WebDE;

namespace MapEditor
{
    [JsType(JsMode.Clr, Filename = "scripts/MapEditor.js")]
    public class Login : jQueryContextBase
    {
        public static void TryLogin()
        {
            string password = J("#password").text();
            string username = J("#username").text();
            MapEditor.Server.GetApikey(username, password, ApiKeyCallback);
            MapEditor.Server.GetGroups(GroupsCallback);
        }

        public static void ApiKeyCallback(JsObject result)
        {
            alert(result["apikey"].ToString());
        }

        public static void GroupsCallback(JsObject result)
        {
            alert(result.toString());
        }
    }
}