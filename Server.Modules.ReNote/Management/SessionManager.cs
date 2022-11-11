using Newtonsoft.Json;
using Server.Common.Utilities;
using Server.ReNote.Data;
using Server.ReNote.Encryption;

namespace Server.ReNote.Management
{
    public class SessionManager
    {
        public const int SID_LENGTH = 9;
        
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

        public static void UpdateSessionTimestamp(string sessionId)
        {
            GlobalSession session = GetSession(sessionId);
            session.RequestTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            Database.Instance["sessions"][sessionId] = JsonConvert.SerializeObject(session);
        }

        public static GlobalSession GetSession(string sessionId)
        {
            if (!SessionExists(sessionId))
                return null;

            string rawSession = ((Document)Database.Instance["sessions"][sessionId]).GetRaw();
            if (!JsonUtil.ValiditateJson(rawSession))
                return null;

            return JsonConvert.DeserializeObject<GlobalSession>(rawSession);
        }

        public static void DeleteSession(string sId)
        {
            Database.Instance["sessions"].RemoveKey(sId);
        }

        public static bool SessionExists(string sId)
        {
            return Database.Instance["sessions"][sId] != null;
        }

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
        [JsonProperty("sessionId")]
        public long SessionId { get; set; }

        [JsonProperty("userId")]
        public long UserId { get; set; }

        [JsonProperty("accountType")]
        public int AccountType { get; set; }
        
        [JsonProperty("authToken")]
        public string AuthToken { get; set; }

        [JsonProperty("requestTimestamp")]
        public long RequestTimestamp { get; set; }

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