using System;
using System.Collections;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace WebDEServerSharp.API
{
    public static class Errors
    {
        /// <summary>
        /// Error Code 10, missing parameter.
        /// </summary>
        /// <param name="paramName">The name of the missing parameter.</param>
        /// /// <param name="ClientRequestObject">The request object to send the error to.</param>
        public static void MissingParameter(string paramName, XCore.Net.WebServer.Asynchronous.HttpRequestStateObject ClientRequestObject)
        {
            Hashtable result = new Hashtable();
            result.Add("E", 10);
            result.Add("M", "Missing parameter named: " + paramName);
            ClientRequestObject.AddContent(JsonConvert.SerializeObject(result));
            ClientRequestObject.CompleteSuccesfulRequest();
        }

        /// <summary>
        /// Error code 11, error processing prameter.
        /// </summary>
        /// <param name="paramName">The errored parameter name.</param>
        /// <param name="ClientRequestObject">The request object to send the error to.</param>
        public static void ErrorProcessingParameter(string paramName, object paramValue, XCore.Net.WebServer.Asynchronous.HttpRequestStateObject ClientRequestObject)
        {
            Hashtable result = new Hashtable();
            result.Add("E", 11);
            result.Add("M", "Error processing parameter named: " + paramName + " with value: " + paramValue.ToString());
            ClientRequestObject.AddContent(JsonConvert.SerializeObject(result));
            ClientRequestObject.CompleteSuccesfulRequest();
        }
    }
}
