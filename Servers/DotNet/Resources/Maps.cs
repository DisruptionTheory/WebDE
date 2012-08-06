using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alchemy.Classes;
using WebDEServerDotNet.Net;
using WebDEServerDotNet.Data;
using Newtonsoft.Json;
using DB = WebDEServerDotNet.Data;
using WebDEServerDotNet.Net;

namespace WebDEServerDotNet.Resources
{
    /// <summary>
    /// Controls requests and updates for map resources.
    /// </summary>
    public static class Maps
    {
        /// <summary>
        /// Initialize the map resource controller and add dispatch functions to dispatch list.
        /// </summary>
        public static void Initialize()
        {
            Server.SetResourceRequest(WebDE.Types.Net.Resource.MAP, mapRequest);
            Server.SetResourceUpdate(WebDE.Types.Net.Resource.MAP, mapUpdate);
        }

        /// <summary>
        /// Fired when a request for a map or maps is received.
        /// </summary>
        /// <param name="parameters">The parameters table.</param>
        /// <param name="ctx">The user context.</param>
        private static void mapRequest(Hashtable parameters, UserContext ctx)
        {
            if (parameters["mapid"].ToString() == "0") Server.Send(DB.Maps.GetAll(), ctx);
            else
            {
                int mapid = int.Parse(parameters["mapid"].ToString());
                Server.Send(DB.Maps.Get(mapid), ctx);
            }
        }

        /// <summary>
        /// Fired when a request to update the map database values is received.
        /// </summary>
        /// <param name="parameters">The parameters table.</param>
        /// <param name="ctx">The user context.</param>
        private static void mapUpdate(Hashtable parameters, UserContext ctx)
        {

        }
    }
}
