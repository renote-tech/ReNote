using Client.Api.Responses;

namespace Client.ReNote
{
    internal class GlobalSession
    {
        public string UserName { get; set; }
        public long SessionId { get; set; }
        public string AuthToken { get; set; }

        public static GlobalSession Create(AuthenticateData data)
        {
            if (data == null)
                return null;

            return new GlobalSession(data.UserName, data.SessionId, data.AuthToken);
        }

        public GlobalSession()
        {

        } 

        public GlobalSession(string userName, long sessionId, string authToken)
        {
            UserName = userName;
            SessionId = sessionId;
            AuthToken = authToken;
        }
    }
}