using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XCore.Net.API.Asynchronous;
using WebDEServerSharp.API.Resources;

namespace WebDEServerSharp.API
{
    /// <summary>
    /// Manages the creation and maintenace of the different API endpoints.
    /// </summary>
    public static class APIController
    {
        private static HttpEndpoint endpoint = new HttpEndpoint(81);

        /// <summary>
        /// Initialize the api endpoint and prepare for requests.
        /// </summary>
        public static void Intialize()
        {
            endpoint.BindLocation("/resources/maps", new Maps());
        }
    }
}
