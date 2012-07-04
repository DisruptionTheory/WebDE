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
            //Get the game ID
            //int gameID = int.Parse(ClientRequestObject.parameters["gameid"].ToString());
            int gameID = 0;

            ClientRequestObject.AddContent(JsonConvert.SerializeObject(Data.Maps.GetAll(gameID)));

            //complete request
            ClientRequestObject.CompleteSuccesfulRequest();
        }
    }
}
