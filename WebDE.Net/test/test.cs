using System;
using System.Collections.Generic;
using SharpKit.JavaScript;
using SharpKit.Html;


namespace WebDE.Net
{
    [JsType(JsMode.Clr, Filename = "test.js")]
    public class test : HtmlContextBase
    {
        public static void StartTest()
        {
            NetworkClient client = new NetworkClient("localhost", 81);
            client.Connect();
            client.OnDisconnect += new ConnectionStateChangeEventHandler(client_OnDisconnect);
        }

        static void client_OnDisconnect()
        {
            alert("hello!");
        }
    }
}