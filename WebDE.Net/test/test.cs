using System;
using System.Collections.Generic;
using SharpKit.JavaScript;
using SharpKit.Html4;
using Types = WebDE.Types;

namespace WebDE.Net
{
    [JsType(JsMode.Clr, Filename = "test.js")]
    public class test : HtmlContextBase
    {
        private static NetworkClient client;
        public static void StartTest()
        {
            client = new NetworkClient("localhost", 81);
            client.Connect();
            client.OnConnect += new ConnectionStateChangeEventHandler(client_OnConnect);
            client.OnDisconnect += new ConnectionStateChangeEventHandler(client_OnDisconnect);
            client.OnReceive += new OnReceiveEventHandler(client_OnReceive);

        }

        static void client_OnReceive(Dictionary<string, object> message)
        {
            document.getElementById("output").innerText = JSON.stringify(message);
        }

        static void client_OnConnect()
        {
            var req = new { action = Types.Net.Action.GET, type = Types.Net.Resources.MAP, mapid = 1 };
            client.Send(req);
        }

        static void client_OnDisconnect()
        {
            document.getElementById("output").innerText = "DISCONNECTED!";
        }
    }
}