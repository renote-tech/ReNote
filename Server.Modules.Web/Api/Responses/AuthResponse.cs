using Newtonsoft.Json;

namespace Server.Web.Api.Responses
{
    internal class AuthResponse
    {
        [JsonProperty("sessionId")]
        public long SessionId { get; set; }

        [JsonProperty("userId")]
        public long UserId { get; set; }

        [JsonProperty("accountType")]
        public int AccountType { get; set; }

        [JsonProperty("authToken")]
        public string AuthToken { get; set; }
    }
}