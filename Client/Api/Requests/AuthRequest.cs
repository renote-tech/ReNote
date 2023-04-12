using Newtonsoft.Json;

namespace Client.Api.Requests
{
    internal class AuthRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}