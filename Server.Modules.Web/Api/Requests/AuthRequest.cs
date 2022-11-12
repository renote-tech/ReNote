using Newtonsoft.Json;

namespace Server.Web.Api.Requests
{
    internal class AuthRequest
    {
        /// <summary>
        /// The username of the <see cref="AuthRequest"/>.
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// The password of the <see cref="AuthRequest"/>.
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}