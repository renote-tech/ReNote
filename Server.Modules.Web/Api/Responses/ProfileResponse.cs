using Newtonsoft.Json;

namespace Server.Web.Api.Responses
{
    public class ProfileResponse
    {
        /// <summary>
        /// The real name of the <see cref="ProfileResponse"/>.
        /// </summary>
        [JsonProperty("realName")]
        public string RealName { get; set; }

        /// <summary>
        /// The team id of the <see cref="ProfileResponse"/>.
        /// </summary>
        [JsonProperty("teamId")]
        public int TeamId { get; set; }

        /// <summary>
        /// The profile picture of the <see cref="ProfileResponse"/>.
        /// </summary>
        [JsonProperty("profilePicture")]
        public string ProfilePicture { get; set; }

        /// <summary>
        /// The email of the <see cref="ProfileResponse"/>.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// The phone number of the <see cref="ProfileResponse"/>.
        /// </summary>
        [JsonProperty("phone")]
        public string Phone { get; set; }

        /// <summary>
        /// The birthday of the <see cref="ProfileResponse"/>.
        /// </summary>
        [JsonProperty("birthday")]
        public string Birthday { get; set; }

        /// <summary>
        /// The last connection time of the <see cref="ProfileResponse"/>.
        /// </summary>
        [JsonProperty("lastConnection")]
        public long LastConnection { get; set; }
    }
}