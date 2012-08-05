using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebDEServerSharp.Net;

namespace WebDEServerSharp
{

    /// <summary>
    /// Manages global application startup and maintenance.
    /// </summary>
    public static class Application
    {
        /// <summary>
        /// Initialize the WebDE Server
        /// </summary>
        public static void Initialize()
        {
            Server.Intitialize();
        }
    }
}
