using SharpKit.JavaScript;
using SharpKit.Html;
using SharpKit.jQuery;
using WebDE.Net;

namespace MapEditor
{
    [JsType(JsMode.Clr, Filename = "scripts/MapEditor.js")]
    public class Views
    {
        public static string Login
        {
            get { return "Login"; }
        }

        public static string Editor
        {
            get { return "Editor"; }
        }

        /// <summary>
        /// Load a view into the body.
        /// </summary>
        /// <param name="view">The string name of the view to load.</param>
        public static void LoadView(string view)
        {
            jQueryContext.J("#" + view).css("display", "block");
        }
    }
}