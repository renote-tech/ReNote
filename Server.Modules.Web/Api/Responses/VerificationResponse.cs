using Newtonsoft.Json;

namespace Server.Web.Api.Responses
{
    public class VerificationResponse
    {
        /// <summary>
        /// The user id of the <see cref="VerificationResponse"/>.
        /// </summary>
        [JsonProperty("userId")]
        public long UserId { get; set; }
        /// <summary>
        /// The session id of the <see cref="VerificationResponse"/>.
        /// </summary>
        [JsonProperty("sessionId")]
        public long SessionId { get; set; }
        /// <summary>
        /// The API response of the <see cref="VerificationResponse"/>.
        /// </summary>
        [JsonProperty("response")]
        public ApiResponse Response { get; set; }
    }
}