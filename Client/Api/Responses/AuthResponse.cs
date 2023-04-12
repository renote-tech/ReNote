using Newtonsoft.Json;

namespace Client.Api.Responses
{
    internal class AuthResponse : Response
    {
        [JsonProperty("data", Order = 10)]
        private AuthData m_Data;

        public void SetData(AuthData data)
        {
            m_Data = data;
        }

        public AuthData GetData()
        {
            return m_Data;
        }
    }

    public class AuthData
    {
        [JsonProperty("sessionId")]
        private long m_SessionId;

        [JsonProperty("userId")]
        private long m_UserId;

        [JsonProperty("accountType")]
        private int m_AccountType;

        [JsonProperty("authToken")]
        private string m_AuthToken;

        public void SetSessionId(long sessionId)
        {
            m_SessionId = sessionId;
        }

        public long GetSessionId()
        {
            return m_SessionId;
        }

        public void SetUserId(long userId)
        {
            m_UserId = userId;
        }

        public long GetUserId()
        {
            return m_UserId;
        }

        public void SetAccountType(int accountType) 
        {
            m_AccountType = accountType;
        }
        public int GetAccountType()
        {
            return m_AccountType;
        }

        public void SetAuthToken(string authToken)
        {
            m_AuthToken = authToken;
        }

        public string GetAuthToken()
        {
            return m_AuthToken;
        }
    }
}