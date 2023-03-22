using Newtonsoft.Json;
using Client.Api;
using Client.Api.Responses;

namespace Client.ReNote
{
    internal class Session
    {
        [JsonProperty("userId")]
        public long UserId { get; set; }

        [JsonProperty("sessionId")]
        public long SessionId { get; set; }

        [JsonProperty("accountType")]
        public int AccountType { get; set; }

        [JsonProperty("authToken")]
        public string AuthToken { get; set; }

        [JsonProperty("realName")]
        public string RealName { get; set; }

        [JsonProperty("profilePicture")]
        public string ProfilePicture { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string PhoneNumber { get; set; }

        [JsonProperty("birthday")]
        public string Birthday { get; set; }

        [JsonProperty("lastConnection")]
        public long LastConnection { get; set; }

        public static async Task<Session> CreateAsync(AuthData data)
        {
            HttpResponseMessage profileResponse = await ApiClient.GetProfileAsync(data.GetSessionId(), data.GetAuthToken());
            if (profileResponse.StatusCode != System.Net.HttpStatusCode.OK)
                return null;

            string body = await profileResponse.Content.ReadAsStringAsync();
            ProfileResponse profileData = JsonConvert.DeserializeObject<ProfileResponse>(body);

            return new Session()
            {
                UserId         = data.GetUserId(),
                SessionId      = data.GetSessionId(),
                AccountType    = data.GetAccountType(),
                AuthToken      = data.GetAuthToken(),
                RealName       = profileData.GetData().GetRealName(),
                ProfilePicture = profileData.GetData().GetProfilePicture(),
                Email          = profileData.GetData().GetEmail(),
                PhoneNumber    = profileData.GetData().GetPhoneNumber(),
                Birthday       = profileData.GetData().GetBirthday(),
                LastConnection = profileData.GetData().GetLastConnection()
            };
        }
    }
}