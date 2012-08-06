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
using WebDEServerDotNet.Users;


namespace WebDEServerDotNet.Net
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
        private static Dictionary<WebDE.Types.Net.Resource, Action<Hashtable, UserContext>> requestResourceDispatch = new Dictionary<WebDE.Types.Net.Resource, Action<Hashtable, UserContext>>();

        /// <summary>
        /// The dispatch table for resource updates. 
        /// </summary>
        private static Dictionary<WebDE.Types.Net.Resource, Action<Hashtable, UserContext>> updateResourceDispatch = new Dictionary<WebDE.Types.Net.Resource, Action<Hashtable, UserContext>>();
        
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

        /// <summary>
        /// Event fired when a new client starts connecting to the server.
        /// </summary>
        /// <param name="ctx">The user context.</param>
        private static void OnConnect(UserContext ctx)
        {
            //client is not yet connected, nothing to do
        }

        /// <summary>
        /// Event fired when a new user is fully connected to the server.
        /// </summary>
        /// <param name="ctx">The user context.</param>
        private static void OnConnected(UserContext ctx)
        {
            WebDEServerDotNet.Users.UserControl.RegisterUser(ctx);
        }

        /// <summary>
        /// Event fired when a user disconnects from the server.
        /// </summary>
        /// <param name="ctx">The user context.</param>
        private static void OnDisconnect(UserContext ctx)
        {
            WebDEServerDotNet.Users.UserControl.DeregisterUser(ctx);
        }

        /// <summary>
        /// Event fired when data is sent from the server to the client.
        /// </summary>
        /// <param name="ctx">The user context.</param>
        private static void OnSend(UserContext ctx)
        {
            //nothing to do
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

            //dispatch based on action
            switch ((WebDE.Types.Net.Action)action)
            {
                case WebDE.Types.Net.Action.GET:
                    int gettype = int.Parse(json["type"].ToString());
                    requestResourceDispatch[(WebDE.Types.Net.Resource)gettype](json, ctx);
                    break;
                case WebDE.Types.Net.Action.SET:
                    int settype = int.Parse(json["type"].ToString());
                    updateResourceDispatch[(WebDE.Types.Net.Resource)settype](json, ctx);
                    break;
                case WebDE.Types.Net.Action.KEY:
                    WebDEServerDotNet.Security.API.RequestApiKey(json, ctx);
                    break;
                case WebDE.Types.Net.Action.GRP:
                    Send(Security.API.GetGroups(UserControl.GetUserByContext(ctx)), ctx);
                    break;
                default:
                    //TODO: unknown action, send back error
                    break;
            }
        }

        /// <summary>
        /// Set the function that gets invoked when the specified resource gets requested.
        /// </summary>
        /// <param name="resource">The resource that got requested.</param>
        /// <param name="function">The function to be invoked.</param>
        public static void SetResourceRequest(WebDE.Types.Net.Resource resource, Action<Hashtable, UserContext> function)
        {
            requestResourceDispatch.Add(resource, function);
        }

        /// <summary>
        /// Set the function that gets invoked when the specified resource gets updated.
        /// </summary>
        /// <param name="resource">The resource to be updated</param>
        /// <param name="function">The function to be invoked.</param>
        public static void SetResourceUpdate(WebDE.Types.Net.Resource resource, Action<Hashtable, UserContext> function)
        {
            updateResourceDispatch.Add(resource, function);
        }

        /// <summary>
        /// Send the specified message to the target user. 
        /// This method will JSON stringify the message object.
        /// </summary>
        /// <param name="message">The message object.</param>
        /// <param name="ctx">The user context.</param>
        public static void Send(object message, UserContext ctx)
        {
            string msg = JsonConvert.SerializeObject(message);
            ctx.Send(msg);
        }

        /// <summary>
        /// Send the specified message to the target user. 
        /// This method will JSON stringify the message object.
        /// </summary>
        /// <param name="message">The message object.</param>
        /// <param name="user">The user object.</param>
        public static void Send(object message, User user)
        {
            string msg = JsonConvert.SerializeObject(message);
            user.UserContext.Send(msg);
        }


    }
}
