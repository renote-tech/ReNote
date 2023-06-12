using Newtonsoft.Json;

namespace Server.Web.Api.Requests
{
    internal class PasswordRequest
    {
        /// <summary>
        /// The current password of the <see cref="PasswordRequest"/>.
        /// </summary>
        [JsonProperty("currentPassword")]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// The new password of the <see cref="PasswordRequest"/>.
        /// </summary>
        [JsonProperty("newPassword")]
        public string NewPassword { get; set; }
    }
}