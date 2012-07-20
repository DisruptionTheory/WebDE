using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XCore.Net.API.Asynchronous;
using WebDEServerSharp.Data;
using Newtonsoft.Json;

namespace WebDEServerSharp.API.Resources
{
    /// <summ ary>
    /// Manages all api requests for map resources.
    /// </summary>
    public class Maps : APILocation
    {
        public void Request(XCore.Net.WebServer.Asynchronous.HttpRequestStateObject ClientRequestObject)
        {
            //map id
            int mapID;

            if (!ClientRequestObject.parameters.ContainsKey("mapid"))
            {
                Errors.MissingParameter("mapid", ClientRequestObject);
                return;
            }

            if (int.TryParse(ClientRequestObject.parameters["mapid"].ToString(), out mapID))
            {
                //query database for map content, if map id is 0, get all
                if (mapID == 0)
                {
                    ClientRequestObject.AddContent(JsonConvert.SerializeObject(Data.Maps.GetAll()));
                }
                else
                {
                    ClientRequestObject.AddContent(JsonConvert.SerializeObject(Data.Maps.Get(mapID)));
                }

                //complete request
                ClientRequestObject.CompleteSuccesfulRequest();
            }
            else
            {
                Errors.ErrorProcessingParameter("mapid", ClientRequestObject.parameters["mapid"], ClientRequestObject);
            }
        }
    }
}
