using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Alchemy;
using Alchemy.Classes;


namespace WebDEServerSharp.Net
{
    public static class Server
    {
        /// <summary>
        /// The internal web socket server.
        /// </summary>
        private static WebSocketServer wsserver;
        
        /// <summary>
        /// Initializes the WebSocket server and begins accepting connections.
        /// </summary>
        public static void Intitialize()
        {
            wsserver = new WebSocketServer(Config.ServerPort, IPAddress.Any);
            wsserver.OnConnect = OnConnect;
            wsserver.OnConnected = OnConnected;
            wsserver.OnDisconnect = OnDisconnect;
            wsserver.OnReceive = OnReceive;
            wsserver.OnSend = OnSend;
        }

        private static void OnConnect(UserContext ctx)
        {
        }

        private static void OnConnected(UserContext ctx)
        {
        }

        private static void OnDisconnect(UserContext ctx)
        {
        }

        private static void OnSend(UserContext ctx)
        {
        }

        private static void OnReceive(UserContext ctx)
        {
        }

    }
}
