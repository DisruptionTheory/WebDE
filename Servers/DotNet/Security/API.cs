using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alchemy.Classes;
using XCore.Helpers;
using WebDEServerDotNet;
using WebDEServerDotNet.Users;
using WebDEServerDotNet.Net;

namespace WebDEServerDotNet.Security
{
    /// <summary>
    /// Handles all API key functionality.
    /// </summary>
    public static class API
    {
        private static ConcurrentDictionary<string, WebDE.Types.AccessGroup[]> accessList = new ConcurrentDictionary<string, WebDE.Types.AccessGroup[]>();
        /// <summary>
        /// Service a users request for an API key.
        /// </summary>
        /// <param name="parameters">The hash table of parameters associated with the api key request.</param>
        /// <param name="ctx">The user context.</param>
        public static void RequestApiKey(Hashtable parameters, UserContext ctx)
        {
            User user = Users.UserControl.GetUserByContext(ctx);
            string key = user.ApiKey;
            string username = parameters["user"].ToString();
            string password = parameters["pass"].ToString();
            if (user.ApiKey != String.Empty)
            {
                if (accessList.ContainsKey(user.ApiKey)) accessList[user.ApiKey] = getGroups(username, password);
                else
                {
                    key = Generator.RandomAlphaNumeric(Config.ApiKeyLength);
                    user.ApiKey = key;
                    accessList.TryAdd(key, getGroups(username, password));
                }
            }
            else
            {
                key = Generator.RandomAlphaNumeric(Config.ApiKeyLength);
                user.ApiKey = key;
                accessList.TryAdd(key, getGroups(username, password));
            }
            DeliverApiKey(user);
        }

        /// <summary>
        /// Get the list of groups the user with he specified name and password belongs to.
        /// </summary>
        /// <param name="username">The users username.</param>
        /// <param name="password">The users password.</param>
        /// <returns>An array of groups the specified user belongs to.</returns>
        private static WebDE.Types.AccessGroup[] getGroups(string username, string password)
        {
            //empty username or password is generic player login
            if(username ==  string.Empty || password == string.Empty)
            {
                return new WebDE.Types.AccessGroup[] {WebDE.Types.AccessGroup.PLAYER};
            }

            //TODO: IMPLEMENT LOGIN FUNCTIONAILTY
            //TEMPORARY
            if(username == "webde" && password == "webde")
            {
                return new WebDE.Types.AccessGroup[] {WebDE.Types.AccessGroup.SUPER_USER};
            }else{
                return new WebDE.Types.AccessGroup[] {WebDE.Types.AccessGroup.PLAYER};
            }
        }

        /// <summary>
        /// Get all groups that the target user belongs to.
        /// </summary>
        /// <param name="user">The target user.</param>
        /// <returns>All groups that the target user belongs to.</returns>
        public static WebDE.Types.AccessGroup[] GetGroups(User user)
        {
            if (user.ApiKey == string.Empty) return new WebDE.Types.AccessGroup[] { };
            if (!accessList.ContainsKey(user.ApiKey)) return new WebDE.Types.AccessGroup[] { };
            return accessList[user.ApiKey];
        }

        /// <summary>
        /// Sends an API key update message to a client. 
        /// </summary>
        /// <param name="user">The user to send the key to.</param>
        public static void DeliverApiKey(User user)
        {
            var message = new
            {
                action = WebDE.Types.Net.Action.KEY,
                apikey = user.ApiKey
            };
            Server.Send(message, user);
        }
    }
}
