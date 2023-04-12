using Newtonsoft.Json;

namespace Server.ReNote.Data
{
    public class User
    {
        /// <summary>
        /// The real name of the <see cref="User"/>.
        /// </summary>
        [JsonProperty("realName")]
        public string RealName { get; set; }

        /// <summary>
        /// The login name of the <see cref="User"/>.
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// The profile picture of the <see cref="User"/>.
        /// </summary>
        [JsonProperty("profilePicture")]
        public string ProfilePicture { get; set; }

        /// <summary>
        /// The user id of the <see cref="User"/>.
        /// </summary>
        [JsonProperty("userId")]
        public long UserId { get; set; }

        /// <summary>
        /// The team id of the <see cref="User"/>.
        /// </summary>
        [JsonProperty("teamId")]
        public int TeamId { get; set; }

        /// <summary>
        /// The account type of the <see cref="User"/>.
        /// </summary>
        [JsonProperty("accountType")]
        public int AccountType { get; set; }

        /// <summary>
        /// The email of the <see cref="User"/>.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// The phone number of the <see cref="User"/>.
        /// </summary>
        [JsonProperty("phone")]
        public string Phone { get; set; }

        /// <summary>
        /// The birthday of the <see cref="User"/>.
        /// </summary>
        [JsonProperty("birthday")]
        public string Birthday { get; set; }

        /// <summary>
        /// The last connection time of the <see cref="User"/>.
        /// </summary>
        [JsonProperty("lastConnection")]
        public long LastConnection { get; set; }

        /// <summary>
        /// The secure password of the <see cref="User"/>.
        /// </summary>
        [JsonProperty("securePassword")]
        public string SecurePassword { get; set; }

        /// <summary>
        /// The IV password of the <see cref="User"/>.
        /// </summary>
        [JsonProperty("ivPassword")]
        public byte[] IVPassword { get; set; }
    }
}