using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDEServerDotNet
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
            Config.Initialize();
            Resources.Control.Intitialize();
            Net.Server.Intitialize();
            while (true)
            {
                System.Threading.Thread.Sleep(int.MaxValue);
            }
        }
    }
}
