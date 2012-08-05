using System;
using System.Collections.Generic;
using SharpKit.JavaScript;
using SharpKit.Html;

namespace WebDE.Net
{
    /// <summary>
    /// A delegate type for message received events in the game client web socket.
    /// </summary>
    /// <param name="message">The message received.</param>
    public delegate void OnReceiveEventHandler(string message);

    /// <summary>
    /// A delegate type for connection state change events.
    /// </summary>
    public delegate void ConnectionStateChangeEventHandler();

    [JsType(JsMode.Clr, Filename = "scripts/WebDE.Net.js")]
    public class GameClient
    {
        /// <summary>
        /// The port this game client connects to.
        /// </summary>
        public int Port
        {
            get;
            private set;
        }

        /// <summary>
        /// The host this game client connects to.
        /// </summary>
        public string Host
        {
            get;
            private set;
        }

        /// <summary>
        /// An event fired when the GameClient receives a message from he server.
        /// </summary>
        public event OnReceiveEventHandler OnReceive;

        /// <summary>
        /// An event fired when the GameClient connects to a server.
        /// </summary>
        public event ConnectionStateChangeEventHandler OnConnect;

        /// <summary>
        /// An event fired when the game client disconnects from a server.
        /// </summary>
        public event ConnectionStateChangeEventHandler OnDisconnect;

        /// <summary>
        /// The internal web socket.
        /// </summary>
        private WebSocket socket;

        /// <summary>
        /// Create a new game client to connect to the specified host and port.
        /// </summary>
        /// <param name="port">The port on the host to connect to.</param>
        /// <param name="host">The host to connect to.</param>
        public GameClient(string host, int port = 80)
        {
            Port = port;
            Host = host; 
        }

        /// <summary>
        /// Connect the GameClient.
        /// </summary>
        public void Connect(){
            socket = new WebSocket("ws://" + Host + ":" + Port);
            socket.onopen = onOpen;
            socket.onclose = onClose;
            socket.onmessage = onMessage;
        }

        /// <summary>
        /// Internal event fired when socket is opened.
        /// </summary>
        private void onOpen()
        {
            OnConnect.BeginInvoke(null, null);
        }

        /// <summary>
        /// Internal event fired when socket is closed.
        /// </summary>
        /// <param name="evt">The event passed with the close function from the socket.</param>
        private void onClose(CloseEvent evt)
        {
            OnDisconnect.BeginInvoke(null, null);
        }

        /// <summary>
        /// Internal event fired when the socket receives a message.
        /// </summary>
        /// <param name="evt">The message event object.</param>
        private void onMessage(MessageEvent evt)
        {
            OnReceive.BeginInvoke(evt.data.ToString(), null, null);
        }

        /// <summary>
        /// Internal event fired when the socket has an error.
        /// </summary>
        /// <param name="evt">The error event object.</param>
        private void onError(ErrorEvent evt)
        {
            //TODO: decide how to handle errors (pass error as event, handle internally, etc...)
        }
    }
}