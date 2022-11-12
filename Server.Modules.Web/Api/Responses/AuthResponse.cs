using Newtonsoft.Json;

namespace Server.Web.Api.Responses
{
    internal class AuthResponse
    {
        /// <summary>
        /// The session id of the <see cref="AuthResponse"/>.
        /// </summary>
        [JsonProperty("sessionId")]
        public long SessionId { get; set; }

        /// <summary>
        /// The user id of the <see cref="AuthResponse"/>.
        /// </summary>
        [JsonProperty("userId")]
        public long UserId { get; set; }

        /// <summary>
        /// The account type of the <see cref="AuthResponse"/>.
        /// </summary>
        [JsonProperty("accountType")]
        public int AccountType { get; set; }

        /// <summary>
        /// The auth token of the <see cref="AuthResponse"/>.
        /// </summary>
        [JsonProperty("authToken")]
        public string AuthToken { get; set; }
    }
}