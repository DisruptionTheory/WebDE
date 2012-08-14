﻿using SharpKit.JavaScript;
using SharpKit.Html4;
using SharpKit.jQuery;
using WebDE.Net;
using UITK;

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
            ImageBox box = new ImageBox(100, 200, "http://www.w3schools.com/images/compatible_chrome.gif");
            Surface.AddComponent(box, new UITKPosition(300, 500));
            
            //Server = new GameClient("localhost", 81);
            //Views.LoadView(Views.Login);
        }
    }
}