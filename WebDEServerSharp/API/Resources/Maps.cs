using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XCore.Net.API.Asynchronous;
using WebDEServerSharp.Data;
using Newtonsoft.Json;

namespace WebDEServerSharp.API.Resources
{
    /// <summary>
    /// Manages all api requests for map resources.
    /// </summary>
    public class Maps : APILocation
    {
        public void Request(XCore.Net.WebServer.Asynchronous.HttpRequestStateObject ClientRequestObject)
        {
            //Get the game ID and map id
            int gameID = int.Parse(ClientRequestObject.parameters["gameid"].ToString());
            int mapid = int.Parse(ClientRequestObject.parameters["mapid"].ToString());

            //query database for map content, if map id is 0, get all
            ClientRequestObject.AddContent(JsonConvert.SerializeObject(Data.Maps.GetAll(gameID)));

            //complete request
            ClientRequestObject.CompleteSuccesfulRequest();
        }
    }
}
