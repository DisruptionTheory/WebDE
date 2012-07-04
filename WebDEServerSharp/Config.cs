using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XCore.Configuration;

namespace WebDEServerSharp
{
    /// <summary>
    /// Controls I/O to the configuration file.
    /// </summary>
    public static class Config
    {
        private static Properties propertiesFile = new Properties("webde.conf");

        /// <summary>
        /// The URL of the database server.
        /// </summary>
        public static string DatabaseLocation
        {
            get;
            private set;
        }

        /// <summary>
        /// The name of the WebDE database.
        /// </summary>
        public static string DatabaseName
        {
            get;
            private set;
        }

        /// <summary>
        /// The username used to access the Database.
        /// </summary>
        public static string DatabaseUser
        {
            get;
            private set;
        }

        /// <summary>
        /// The password used to access the Database.
        /// </summary>
        public static string DatabasePassword
        {
            get;
            private set;
        }

        static Config()
        {
            DatabaseLocation = propertiesFile["dblocation"];
            DatabaseName = propertiesFile["dbname"];
            DatabaseUser = propertiesFile["dbuser"];
            DatabasePassword = propertiesFile["dbpass"];
        }
    }
}
