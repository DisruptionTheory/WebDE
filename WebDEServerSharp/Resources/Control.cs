using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebDEServerSharp.Net;
using WebDEServerSharp.Data;

namespace WebDEServerSharp.Resources
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
