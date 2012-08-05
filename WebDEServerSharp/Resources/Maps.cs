using System;
using System.Collections;
using System.Linq;
using System.Text;
using Alchemy.Classes;
using WebDEServerSharp.Net;
using WebDEServerSharp.Data;
using Newtonsoft.Json;
using DB = WebDEServerSharp.Data;

namespace WebDEServerSharp.Resources
{
    /// <summary>
    /// Controls requests and updates for map resources.
    /// </summary>
    public static class Maps
    {
        public static void Initialize()
        {
            Server.SetResourceRequest(WebDETypes.Net.Resources.MAP, mapRequest);
            Server.SetResourceUpdate(WebDETypes.Net.Resources.MAP, mapUpdate);
        }

        private static void mapRequest(Hashtable parameters, UserContext ctx)
        {
            if (parameters["mapid"].ToString() == "0")
            {
                ctx.Send(JsonConvert.SerializeObject(DB.Maps.GetAll()));
            }
            else
            {
                int mapid = int.Parse(parameters["mapid"].ToString());
                ctx.Send(JsonConvert.SerializeObject(DB.Maps.Get(mapid)));
            }
        }

        private static void mapUpdate(Hashtable parameters, UserContext ctx)
        {

        }
    }
}
