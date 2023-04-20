using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Server.Common.Encryption;
using Server.Common.Utilities;
using Server.ReNote.Data;
using Server.ReNote.Encryption;
using Server.ReNote.Utilities;

namespace Server.ReNote.Management
{
    public class SessionManager
    {
        /// <summary>
        /// Returns a new <see cref="Session"/> instance.
        /// </summary>
        /// <param name="userId">The user id of the <see cref="User"/>.</param>
        /// <returns><see cref="Session"/></returns>
        public static async Task<Session> CreateSessionAsync(long userId)
        {
            long sessionId = CreateSessionId();
            string authToken = await ReNoteToken.GenerateAsync(sessionId);
            string sha256AuthToken = await Sha256.ComputeStringAsync(authToken);

            User userData = UserManager.GetUser(userId);

            long connectionTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Session internalSession = new Session(sessionId, userId, sha256AuthToken, userData.AccountType)
            {
                RequestTimestamp = connectionTimestamp,
                Connection       = connectionTimestamp
            };

            DatabaseUtil.Set(Constants.DB_ROOT_SESSIONS, sessionId.ToString(), internalSession);
            return new Session(sessionId, userId, authToken, userData.AccountType);
        }

        /// <summary>
        /// Updates the request timestamp.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public static void UpdateTimestamp(long sessionId)
        {
            Session session = GetSession(sessionId);
            session.RequestTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            DatabaseUtil.Set(Constants.DB_ROOT_SESSIONS, session.SessionId.ToString(), session);
        }

        /// <summary>
        /// Returns an existing session.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns><see cref="Session"/></returns>
        public static Session GetSession(long sessionId)
        {
            return DatabaseUtil.GetAs<Session>(Constants.DB_ROOT_SESSIONS, sessionId.ToString());
        }

        /// <summary>
        /// Deletes a session.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public static void DeleteSession(long sessionId)
        {
            Session session = GetSession(sessionId);
            User user = UserManager.GetUser(session.UserId);
            user.LastConnection = session.Connection;

            DatabaseUtil.Set(Constants.DB_ROOT_USERS, user.UserId.ToString(), user);
            DatabaseUtil.Remove(Constants.DB_ROOT_SESSIONS, sessionId.ToString());
        }

        /// <summary>
        /// Returns whether the session exists.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool SessionExists(long sessionId)
        {
            return DatabaseUtil.ItemExists(Constants.DB_ROOT_SESSIONS, sessionId.ToString());
        }

        /// <summary>
        /// Deletes sessions if they are expired.
        /// </summary>
        public static void Clean(bool checkExpiring = true)
        {
            string[] sessions = DatabaseUtil.GetValues(Constants.DB_ROOT_SESSIONS);
            for (int i = 0; i < sessions.Length; i++)
            {
                string rawSession = sessions[i];
                if (!JsonUtil.ValiditateJson(rawSession))
                    continue;

                Session session = JsonConvert.DeserializeObject<Session>(rawSession);
                if (checkExpiring)
                    session.HasExpired();
                else
                    DeleteSession(session.SessionId);
            }
        }

        /// <summary>
        /// Returns a session id.
        /// </summary>
        /// <returns><see cref="long"/></returns>
        private static long CreateSessionId()
        {
            long minSid = (long)Math.Pow(10, Constants.SID_LENGTH - 1);
            long maxSid = (long)Math.Pow(10, Constants.SID_LENGTH) - 1;
            long sId    = new Random().NextInt64(minSid, maxSid);
            
            if (SessionExists(sId))
                return CreateSessionId(); 

            return sId;
        }

    }

    public class Session
    {
        /// <summary>
        /// The session id of the <see cref="Session"/>.
        /// </summary>
        [JsonProperty("sessionId")]
        public long SessionId { get; set; }

        /// <summary>
        /// The user id of the <see cref="Session"/>.
        /// </summary>
        [JsonProperty("userId")]
        public long UserId { get; set; }

        /// <summary>
        /// The account type of the <see cref="Session"/>.
        /// </summary>
        [JsonProperty("accountType")]
        public int AccountType { get; set; }

        /// <summary>
        /// The auth token of the <see cref="Session"/>.
        /// </summary>
        [JsonProperty("authToken")]
        public string AuthToken { get; set; }

        /// <summary>
        /// The request timestamp of the <see cref="Session"/>.
        /// </summary>
        [JsonProperty("requestTimestamp")]
        public long RequestTimestamp { get; set; }

        /// <summary>
        /// The connection timestamp of the <see cref="Session"/>.
        /// </summary>
        [JsonProperty("connection")]
        public long Connection { get; set; }

        public Session()
        { }

        public Session(long sessionId, long userId, string authToken, int accountType)
        {
            SessionId   = sessionId;
            UserId      = userId;
            AccountType = accountType;
            AuthToken   = authToken;
        }

        /// <summary>
        /// Returns whether the session has expired or not.
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        public bool HasExpired()
        {
            bool expired = false;
            if ((RequestTimestamp + 600000) < DateTimeOffset.Now.ToUnixTimeMilliseconds())
                expired = true;

            if ((Connection + 1800000) < DateTimeOffset.Now.ToUnixTimeMilliseconds())
                expired = true;

            if (expired)
                SessionManager.DeleteSession(SessionId);

            return expired;
        }
    }
}