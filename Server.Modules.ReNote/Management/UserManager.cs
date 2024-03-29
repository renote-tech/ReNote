﻿using System.Collections.Generic;
using Server.ReNote.Data;
using Server.ReNote.Helpers;

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

            return DatabaseHelper.GetAs<User>(Constants.DB_ROOT_USERS, userId.ToString());
        }

        /// <summary>
        /// Returns an user instance.
        /// </summary>
        /// <param name="userId">The user id of the <see cref="User"/>.</param>
        /// <returns><see cref="User"/></returns>
        public static User GetUser(long userId)
        {
            return DatabaseHelper.GetAs<User>(Constants.DB_ROOT_USERS, userId.ToString());
        }

        /// <summary>
        /// Returns whether the user exists.
        /// </summary>
        /// <param name="username">The username of the <see cref="User"/>.</param>
        /// <returns><see cref="long"/>/returns>
        public static long GetUserId(string username)
        {
            if (ValidateUserId(username))
            {
                long userId = long.Parse(username);
                return UserExists(userId) ? userId : -1;
            }

            Dictionary<string, string> items = DatabaseHelper.GetItems(Constants.DB_ROOT_USERS);
            if (items == null || items.Count == 0)
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
            if (DatabaseHelper.ItemExists(Constants.DB_ROOT_USERS, userId.ToString()))
                return true;

            return false;
        }

        /// <summary>
        /// Returns whether the <see cref="string"/> is a valid <see cref="long"/> value.
        /// </summary>
        /// <param name="data">The <see cref="string"/> to be proceeded.</param>
        /// <returns><see cref="bool"/></returns>
        private static bool ValidateUserId(string data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (!char.IsDigit(data[i]))
                    return false;
            }

            return long.TryParse(data, out _);
        }
    }
}