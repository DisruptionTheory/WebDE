using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using WebDEServerDotNet.Security;
using Alchemy.Classes;

namespace WebDEServerDotNet.Users
{
    public static class UserControl
    {
        /// <summary>
        /// The user list. Keyed by connection address. Addresses contain port information.
        /// </summary>
        private static ConcurrentDictionary<string, User> users = new ConcurrentDictionary<string, User>();

        /// <summary>
        /// Registers a user with the system according to the given Alchemy Context.
        /// </summary>
        /// <param name="ctx">The user context.</param>
        public static void RegisterUser(UserContext ctx)
        {
            //create the user object
            User user = new User(ctx);
            //normally we would check the bool being returned from this function to see if it was added
            //but really the only time it wouldn't be added is if it already existed, which is ok in this case. 
            users.TryAdd(ctx.ClientAddress.ToString(), user);
        }

        /// <summary>
        /// Deregister the user with the given Alchemy Context.
        /// </summary>
        /// <param name="ctx">The user context.</param>
        public static void DeregisterUser(UserContext ctx)
        {
            User user;
            bool successful = users.TryRemove(ctx.ClientAddress.ToString(), out user);
            if (successful)
            {
                user.IsConnected = false;
            }
        }

        /// <summary>
        /// Retrieve a user by their context.
        /// </summary>
        /// <param name="ctx">The users alchemy context.</param>
        /// <returns>The user object for the user.</returns>
        public static User GetUserByContext(UserContext ctx)
        {
            User user;
            users.TryGetValue(ctx.ClientAddress.ToString(), out user);
            return user;
        }

        /// <summary>
        /// Checks if the target user is in the target group.
        /// </summary>
        /// <param name="group">The target group.</param>
        /// <param name="user">The target user.</param>
        /// <returns>Boolean indicating whether or not user is in target group.</returns>
        public static bool InGroup(WebDE.Types.AccessGroup group, User user)
        {
            if (API.GetGroups(user).Contains(group)) return true;
            return false;
        }
    }
}
