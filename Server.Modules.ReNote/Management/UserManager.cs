using Newtonsoft.Json;
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

            return GetUser(userId);
        }

        /// <summary>
        /// Returns an user instance.
        /// </summary>
        /// <param name="userId">The user id of the <see cref="User"/>.</param>
        /// <returns><see cref="User"/></returns>
        public static User GetUser(long userId)
        {
            Document userDocument = DatabaseUtil.Get(Constants.DB_ROOT_USERS, userId.ToString());
            if (userDocument == null)
                return null;

            return JsonConvert.DeserializeObject<User>(userDocument.GetRaw());
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
;
            Dictionary<string, Document> documents = DatabaseUtil.GetDictionary(Constants.DB_ROOT_USERS);
            if (documents.Count == 0)
                return -1;

            for(int i = 0; i < documents.Count; i++)
            {
                if (documents.ElementAt(i).Value == null)
                    continue;

                User user = GetUser(documents.ElementAt(i).Key);
                if (user.Username == username)
                    return user.UserId;
            }

            return -1;
        }

        /// <summary>
        /// Returns whether the user exists.
        /// </summary>
        /// <param name="userId">The user id of the <see cref="User"/>.</param>
        /// <returns><see cref="bool"/></returns>
        private static bool UserExists(long userId)
        {
            if (DatabaseUtil.DocumentExists(Constants.DB_ROOT_USERS, userId.ToString()))
                return true;

            return false;
        }
    }
}