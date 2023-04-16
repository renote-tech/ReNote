using System.Collections.Generic;
using Server.Common.Utilities;
using Server.ReNote.Data;
using Server.ReNote.Utilities;

namespace Server.ReNote.Management
{
    public class UserManager
    {
        /// <summary>
        /// Returns an user instance.
        /// </summary>
        /// <param name="username">The username of the <see cref="User"/>.</param>
        /// <returns><see cref="User"/></returns>
        public static User GetUser(string username)
        {
            long userId = GetUserId(username);
            if (userId == -1)
                return null;

            return DatabaseUtil.GetAs<User>(Constants.DB_ROOT_USERS, userId.ToString());
        }

        /// <summary>
        /// Returns an user instance.
        /// </summary>
        /// <param name="userId">The user id of the <see cref="User"/>.</param>
        /// <returns><see cref="User"/></returns>
        public static User GetUser(long userId)
        {
            return DatabaseUtil.GetAs<User>(Constants.DB_ROOT_USERS, userId.ToString());
        }

        /// <summary>
        /// Returns whether the user exists.
        /// </summary>
        /// <param name="username">The username of the <see cref="User"/>.</param>
        /// <returns><see cref="long"/>/returns>
        public static long GetUserId(string username)
        {
            if (StringUtil.ContainsDigitsOnly(username) && NumberUtil.IsSafeLong(username))
            {
                long userId = long.Parse(username);
                return UserExists(userId) ? userId : -1;
            }

            Dictionary<string, string> items = DatabaseUtil.GetItems(Constants.DB_ROOT_USERS);
            if (items.Count == 0)
                return -1;

            if (!items.ContainsKey(username))
                return -1;

            string stringUserId = items[username];
            if (stringUserId != null)
                return long.Parse(stringUserId);

            return -1;
        }

        /// <summary>
        /// Returns whether the user exists.
        /// </summary>
        /// <param name="userId">The user id of the <see cref="User"/>.</param>
        /// <returns><see cref="bool"/></returns>
        private static bool UserExists(long userId)
        {
            if (DatabaseUtil.ItemExists(Constants.DB_ROOT_USERS, userId.ToString()))
                return true;

            return false;
        }
    }
}