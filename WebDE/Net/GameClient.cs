using System;
using System.Collections;
using SharpKit.JavaScript;
using SharpKit.Html4;
using SharpKit.jQuery;

namespace WebDE.Net
{
    [JsType(JsMode.Clr, Filename = "../scripts/WebDE.Net.GameClient.js")]
    public class GameClient
    {
        /// <summary>
        /// The host the game client connects to.
        /// </summary>
        public string Host
        {
            get { return client.Host; }
        }

        /// <summary>
        /// The host port the game client connects to.
        /// </summary>
        public int Port
        {
            get { return client.Port; }
        }

        /// <summary>
        /// This Game Clients API Key.
        /// </summary>
        public string ApiKey
        {
            get;
            private set;
        }

        /// <summary>
        /// The Web Socket network client used by this game client.
        /// </summary>
        private NetworkClient client;

        /// <summary>
        /// We keep a request queue for quick requests.
        /// </summary>
        private MessageQueue requestQueue = new MessageQueue();

        private bool messageInTransit = false;

        /// <summary>
        /// The current callback for the message in transit.
        /// </summary>
        private Action<JsObject> messageCallBack;

        /// <summary>
        /// Create a new game client object with the target host and port.
        /// </summary>
        /// <param name="host">The target host.</param>
        /// <param name="port">The target port.</param>
        public GameClient(string host, int port = 80)
        {
            client = new NetworkClient(host, port);
            client.OnConnect +=new ConnectionStateChangeEventHandler(client_OnConnect);
            client.OnDisconnect += new ConnectionStateChangeEventHandler(client_OnDisconnect);
            client.OnReceive += new OnReceiveEventHandler(client_OnReceive);
            client.Connect();
        }

        private void  client_OnConnect()
        {
 	        
        }

        private void  client_OnDisconnect()
        {
 	        
        }

        void  client_OnReceive(JsObject message)
        {
            messageInTransit = false;
            Action<JsObject> callback = messageCallBack;
            if (requestQueue.Count > 0)
            {
                object[] queueItem = (object[])requestQueue.Dequeue();
                send(queueItem[0], (Action<JsObject>)queueItem[1]);
            }
            callback(message);
        }

        private void send(object message, Action<JsObject> callback)
        {
            if (messageInTransit)
            {
                object[] queueItem = new object[] { message, callback };
                requestQueue.Enqueue(queueItem);
            }
            else
            {
                messageInTransit = true;
                messageCallBack = callback;
                client.Send(message);
            }
        }

        /// <summary>
        /// Get an API Key for this client.
        /// </summary>
        /// <param name="username">The clients username.</param>
        /// <param name="password">The clients password.</param>
        /// <param name="callback">The callback to be called when the API key has returned.</param>
        public void GetApikey(string username, string password, Action<JsObject> callback)
        {
            var obj = new
            {
                action = WebDE.Types.Net.Action.KEY,
                user = username,
                pass = password
            };
            send(obj, callback);
        }


    }
}