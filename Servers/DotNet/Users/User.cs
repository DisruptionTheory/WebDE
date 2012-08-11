using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AC = Alchemy.Classes;

namespace WebDEServerDotNet.Users
{
    public class User
    {
        /// <summary>
        /// The Alchemy User Context for this user.
        /// </summary>
        public AC.UserContext UserContext
        {
            get;
            private set;
        }

        /// <summary>
        /// This users API key.
        /// </summary>
        public string ApiKey
        {
            get;
            internal set;
        }

        /// <summary>
        /// This users user name.
        /// </summary>
        public string UserName
        {
            get;
            internal set;
        }

        /// <summary>
        /// Boolean indicating if the client is connected.
        /// </summary>
        public bool IsConnected
        {
            get;
            internal set;
        }

        /// <summary>
        /// Create a new User object with the given Alchemy Context.
        /// </summary>
        /// <param name="ctx">The users Alchemy Context.</param>
        public User(AC.UserContext ctx)
        {
            ApiKey = String.Empty;
            UserName = String.Empty;
            IsConnected = true;
            UserContext = ctx;
        }

        /// <summary>
        /// Check to see if the current user is in the target group.
        /// </summary>
        /// <param name="group">The target group to check for.</param>
        /// <returns>Boolean indicating if the user is in the target group.</returns>
        public bool InGroup(WebDE.Types.AccessGroup group)
        {
            return UserControl.InGroup(group, this);
        }
    }
}
