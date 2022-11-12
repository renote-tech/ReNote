using Newtonsoft.Json;
using Server.Common.Utilities;
using Server.ReNote.Data;
using Server.ReNote.Encryption;

namespace Server.ReNote.Management
{
    public class SessionManager
    {
        /// <summary>
        /// The length of the session id.
        /// </summary>
        public const int SID_LENGTH = 9;
        
        /// <summary>
        /// Returns a new <see cref="GlobalSession"/> instance.
        /// </summary>
        /// <param name="userId">The user id of the <see cref="User"/>.</param>
        /// <returns><see cref="GlobalSession"/></returns>
        public static GlobalSession CreateSession(long userId)
        {
            long sId = CreateSessionId();
            string authToken = ReNoteSecureToken.Generate(sId);
            string sha256AuthToken = EncryptionUtil.ComputeStringSha256(authToken);

            User userData = UserManager.GetUser(userId);

            GlobalSession session = new GlobalSession(sId, userId, authToken, userData.AccountType);
            Document[] sessions = Database.Instance["sessions"].GetValues();

            for(int i = 0; i < sessions.Length; i++)
            {
                string rawSession = sessions[i].GetRaw();
                if (!JsonUtil.ValiditateJson(rawSession))
                    continue;

                GlobalSession user = JsonConvert.DeserializeObject<GlobalSession>(rawSession);
                if (user.UserId == userId)
                    Database.Instance["sessions"].RemoveKey(sessions[i]);
            }

            GlobalSession internalSession = new GlobalSession(sId, userId, sha256AuthToken, userData.AccountType);
            internalSession.RequestTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            internalSession.Connection = internalSession.RequestTimestamp;

            Database.Instance["sessions"].AddKey(session.SessionId.ToString(), JsonConvert.SerializeObject(internalSession));
            Database.Instance.Save();
            
            return session;
        }

        /// <summary>
        /// Updates the session timestamp.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public static void UpdateSessionTimestamp(string sessionId)
        {
            GlobalSession session = GetSession(sessionId);
            session.RequestTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            Database.Instance["sessions"][sessionId] = JsonConvert.SerializeObject(session);
        }

        /// <summary>
        /// Returns an existing session.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns><see cref="GlobalSession"/></returns>
        public static GlobalSession GetSession(string sessionId)
        {
            if (!SessionExists(sessionId))
                return null;

            string rawSession = ((Document)Database.Instance["sessions"][sessionId]).GetRaw();
            if (!JsonUtil.ValiditateJson(rawSession))
                return null;

            return JsonConvert.DeserializeObject<GlobalSession>(rawSession);
        }

        /// <summary>
        /// Deletes a session.
        /// </summary>
        /// <param name="sId">The session id.</param>
        public static void DeleteSession(string sId)
        {
            Database.Instance["sessions"].RemoveKey(sId);
        }

        /// <summary>
        /// Returns whether the session exists.
        /// </summary>
        /// <param name="sId">The session id.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool SessionExists(string sId)
        {
            return Database.Instance["sessions"][sId] != null;
        }

        /// <summary>
        /// Returns a session id.
        /// </summary>
        /// <returns><see cref="long"/></returns>
        private static long CreateSessionId()
        {
            long minSid = (long)Math.Pow(10, SID_LENGTH - 1);
            long maxSid = (long)Math.Pow(10, SID_LENGTH) - 1;
            long sId    = new Random().NextInt64(minSid, maxSid);

            if (SessionExists(sId.ToString()))
                return CreateSessionId();

            return sId;
        }

    }

    public class GlobalSession
    {
        /// <summary>
        /// The session id of the <see cref="GlobalSession"/>.
        /// </summary>
        [JsonProperty("sessionId")]
        public long SessionId { get; set; }

        /// <summary>
        /// The user id of the <see cref="GlobalSession"/>.
        /// </summary>
        [JsonProperty("userId")]
        public long UserId { get; set; }

        /// <summary>
        /// The account type of the <see cref="GlobalSession"/>.
        /// </summary>
        [JsonProperty("accountType")]
        public int AccountType { get; set; }

        /// <summary>
        /// The auth token of the <see cref="GlobalSession"/>.
        /// </summary>
        [JsonProperty("authToken")]
        public string AuthToken { get; set; }

        /// <summary>
        /// The request timestamp of the <see cref="GlobalSession"/>.
        /// </summary>
        [JsonProperty("requestTimestamp")]
        public long RequestTimestamp { get; set; }

        /// <summary>
        /// The connection timestamp of the <see cref="GlobalSession"/>.
        /// </summary>
        [JsonProperty("connection")]
        public long Connection { get; set; }

        public GlobalSession()
        {

        }

        public GlobalSession(long sessionId, long userId, string authToken, int accountType)
        {
            SessionId = sessionId;
            UserId = userId;
            AccountType = accountType;
            AuthToken = authToken;
        }

        /// <summary>
        /// Returns whether the session has expired or not.
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        public bool HasExpired()
        {
            bool isExpiredSession = false;
            if ((RequestTimestamp + 600000) < DateTimeOffset.Now.ToUnixTimeMilliseconds())
                isExpiredSession = true;

            if (isExpiredSession)
                SessionManager.DeleteSession(SessionId.ToString());

            return isExpiredSession;
        }
    }
}