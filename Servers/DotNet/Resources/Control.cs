using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebDEServerDotNet.Net;
using WebDEServerDotNet.Data;

namespace WebDEServerDotNet.Resources
{
    /// <summary>
    /// Controls the functionality for requests and updates to game resources.
    /// </summary>
    public static class Control
    {
        /// <summary>
        /// Initialize the resource request and update functionality.
        /// </summary>
        public static void Intitialize()
        {
            Maps.Initialize();
        }
    }
}
