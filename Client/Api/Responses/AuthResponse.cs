using Newtonsoft.Json;

namespace Client.Api.Responses
{
    internal class AuthResponse : BaseResponse
    {
        [JsonProperty("data", Order = 10)]
        public AuthData Data { get; set; }
    }

    internal class AuthData
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