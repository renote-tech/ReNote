using Newtonsoft.Json;

namespace Server.Web.Api.Requests
{
    internal class AuthRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}