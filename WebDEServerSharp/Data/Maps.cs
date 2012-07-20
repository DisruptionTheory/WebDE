using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XCore.Data;
using XCore.Data.MySQL;

namespace WebDEServerSharp.Data
{
    /// <summary>
    /// Manages database operions dealing with maps.
    /// </summary>
    public static class Maps
    {

        /// <summary>
        /// Get all maps associated with the specefied game ID.
        /// </summary>
        /// <param name="gameID">The game ID.</param>
        /// <returns>A listing of all query results.</returns>
        public static Dictionary<string, object>[] GetAll()
        {
            return new MySQLAdapter(Config.DatabaseLocation, Config.DatabaseName, Config.DatabaseUser, Config.DatabasePassword).QuickConnect().EasySelect("map").Execute().Results.ToArray();
        }

        public static Dictionary<string, object> Get(int mapID)
        {
            return new MySQLAdapter(Config.DatabaseLocation, Config.DatabaseName, Config.DatabaseUser, Config.DatabasePassword).QuickConnect().EasySelect("map").Where("mapid", Comparison.EQUALS, mapID).Execute().Results[0];
        }

    }
}
