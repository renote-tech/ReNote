using Newtonsoft.Json;
namespace Client.Api.Responses
{
    internal class AuthenticateData
    {
        [JsonProperty("sessionId")]
        public long SessionId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("authToken")]
        public string AuthToken { get; set; }

        public static AuthenticateData Parse(string data)
        {
            return JsonConvert.DeserializeObject<AuthenticateData>(data);
        }
    }
}
