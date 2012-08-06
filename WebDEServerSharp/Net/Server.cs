using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Net;
using Alchemy;
using Alchemy.Classes;
using Newtonsoft.Json;


namespace WebDEServerSharp.Net
{
    public static class Server
    {
        /// <summary>
        /// The internal web socket server.
        /// </summary>
        private static WebSocketServer wsserver;

        /// <summary>
        /// The dispatch table for resource requests. 
        /// </summary>
        private static Dictionary<WebDE.Types.Net.Resources, Action<Hashtable, UserContext>> requestResourceDispatch = new Dictionary<WebDE.Types.Net.Resources, Action<Hashtable, UserContext>>();

        /// <summary>
        /// The dispatch table for resource updates. 
        /// </summary>
        private static Dictionary<WebDE.Types.Net.Resources, Action<Hashtable, UserContext>> updateResourceDispatch = new Dictionary<WebDE.Types.Net.Resources, Action<Hashtable, UserContext>>();
        
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
            wsserver.Start();
        }

        private static void OnConnect(UserContext ctx)
        {
            //client is not yet connected, nothing to do
        }

        private static void OnConnected(UserContext ctx)
        {
            //nothing here yet
        }

        private static void OnDisconnect(UserContext ctx)
        {
            //nothing yet
        }

        private static void OnSend(UserContext ctx)
        {
            //nothing yet
        }

        /// <summary>
        /// Event fired when a data is received from the Alchemy Web sockets server instance.
        /// </summary>
        /// <param name="ctx">The user's connection context</param>
        private static void OnReceive(UserContext ctx)
        {
            //convert the transferred json into a usable hash table
            Hashtable json = JsonConvert.DeserializeObject<Hashtable>(ctx.DataFrame.ToString());
            int action = int.Parse(json["action"].ToString());

            //dispatch based on action, all invocations should be thrown at the thread pool
            if (action == (int)WebDE.Types.Net.Action.GET)
            {
                int type = int.Parse(json["type"].ToString());
                requestResourceDispatch[(WebDE.Types.Net.Resources)type].BeginInvoke(json, ctx, null, null);
            }
            else if (action == (int)WebDE.Types.Net.Action.SET)
            {
                int type = int.Parse(json["type"].ToString());
                updateResourceDispatch[(WebDE.Types.Net.Resources)type].BeginInvoke(json, ctx, null, null);
            }
            else
            {
                //TODO: unknown action, send back error
            }
        }

        /// <summary>
        /// Set the function that gets invoked when the specified resource gets requested.
        /// </summary>
        /// <param name="resource">The resource that got requested.</param>
        /// <param name="function">The function to be invoked.</param>
        public static void SetResourceRequest(WebDE.Types.Net.Resources resource, Action<Hashtable, UserContext> function)
        {
            requestResourceDispatch.Add(resource, function);
        }

        /// <summary>
        /// Set the function that gets invoked when the specified resource gets updated.
        /// </summary>
        /// <param name="resource">The resource to be updated</param>
        /// <param name="function">The function to be invoked.</param>
        public static void SetResourceUpdate(WebDE.Types.Net.Resources resource, Action<Hashtable, UserContext> function)
        {
            updateResourceDispatch.Add(resource, function);
        }

    }
}
