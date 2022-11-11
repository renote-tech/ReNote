using Newtonsoft.Json;

namespace Server.ReNote.Data
{
    public class User
    {
        [JsonProperty("realName")]
        public string RealName { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("profilePicture")]
        public string ProfilePicture { get; set; }

        [JsonProperty("userId")]
        public long UserId { get; set; }

        [JsonProperty("accountType")]
        public int AccountType { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("birthday")]
        public string Birthday { get; set; }

        [JsonProperty("lastConnection")]
        public long LastConnection { get; set; }

        [JsonProperty("securePassword")]
        public string SecurePassword { get; set; }

        [JsonProperty("ivPassword")]
        public byte[] IVPassword { get; set; }
    }
}