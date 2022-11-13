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
            HttpResponseMessage profileResponse = await ApiClient.GetProfileAsync(data.SessionId, data.AuthToken);
            if (profileResponse.StatusCode != System.Net.HttpStatusCode.OK)
                return null;

            string body = await profileResponse.Content.ReadAsStringAsync();
            ProfileResponse profileData = JsonConvert.DeserializeObject<ProfileResponse>(body);

            return new Session()
            {
                UserId         = data.UserId,
                SessionId      = data.SessionId,
                AccountType    = data.AccountType,
                AuthToken      = data.AuthToken,
                RealName       = profileData.Data.RealName,
                ProfilePicture = profileData.Data.ProfilePicture,
                Email          = profileData.Data.Email,
                PhoneNumber    = profileData.Data.PhoneNumber,
                Birthday       = profileData.Data.Birthday,
                LastConnection = profileData.Data.LastConnection
            };
        }
    }
}