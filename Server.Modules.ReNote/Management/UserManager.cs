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
            long userId = UserExists(username);
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
            Document userDocument = DatabaseUtil.Get("users", userId.ToString());
            if (userDocument == null)
                return null;

            return JsonConvert.DeserializeObject<User>(userDocument.GetRaw());
        }

        /// <summary>
        /// Returns whether the user exists.
        /// </summary>
        /// <param name="userId">The user id of the <see cref="User"/>.</param>
        /// <returns><see cref="long"/></returns>
        public static long UserExists(long userId)
        {
            if (DatabaseUtil.DocumentExists("users", userId.ToString()))
                return userId;

            return -1;
        }

        /// <summary>
        /// Returns whether the user exists.
        /// </summary>
        /// <param name="username">The username of the <see cref="User"/>.</param>
        /// <returns><see cref="long"/>/returns>
        public static long UserExists(string username)
        {
            if (StringUtil.ContainsDigitsOnly(username) && NumberUtil.IsSafeLong(username))
                return UserExists(long.Parse(username));

            Dictionary<string, Document> documents = DatabaseUtil.GetDictionary("users");
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
    }
}