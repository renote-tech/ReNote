using Newtonsoft.Json;
using Server.Common.Utilities;
using Server.ReNote.Data;

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
            Document userDocument = (Document)Database.Instance["users"][userId.ToString()];
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
            if (Database.Instance["users"][userId.ToString()] != null)
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

            Document[] documents = Database.Instance["users"].GetValues();
            if (documents.Length == 0)
                return -1;

            for(int i = 0; i < documents.Length; i++)
            {
                if (documents[i] == null)
                    continue;

                if (documents[i].Data.username == username)
                    return documents[i].Data.userId;
            }

            return -1;
        }
    }
}