using System.Net;
using System.Threading.Tasks;
using Client.Api;
using Client.Api.Responses;

namespace Client.ReNote
{
    public class UserSession
    {
        public long UserId { get; set; }
        public int TeamId { get; set; }
        public long SessionId { get; set; }
        public int AccountType { get; set; }
        public string AuthToken { get; set; }
        public string RealName { get; set; }
        public string ProfilePicture { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Birthday { get; set; }
        public long LastConnection { get; set; }
        public Team Team { get; set; }

        public static async Task<UserSession> GetAsync(AuthData data)
        {
            ProfileResponse profileResponse = null;
            TeamProfileResponse teamProfileResponse = null;

            await ApiService.GetProfileAsync(data.GetSessionId(), data.GetAuthToken(), (HttpStatusCode statusCode, ProfileResponse response) =>
            {
                if (statusCode != HttpStatusCode.OK)
                    return;

                profileResponse = response;
            });

            await ApiService.GetTeamProfileAsync(data.GetSessionId(), data.GetAuthToken(), (HttpStatusCode statusCode, TeamProfileResponse response) =>
            {
                if (statusCode != HttpStatusCode.OK)
                    return;

                teamProfileResponse = response;
            });

            if (profileResponse == null || teamProfileResponse == null)
                return null;

            return new UserSession()
            {
                UserId         = data.GetUserId(),
                SessionId      = data.GetSessionId(),
                AccountType    = data.GetAccountType(),
                AuthToken      = data.GetAuthToken(),
                RealName       = profileResponse.GetData().GetRealName(),
                TeamId         = profileResponse.GetData().GetTeamId(),
                ProfilePicture = profileResponse.GetData().GetProfilePicture(),
                Email          = profileResponse.GetData().GetEmail(),
                PhoneNumber    = profileResponse.GetData().GetPhoneNumber(),
                Birthday       = profileResponse.GetData().GetBirthday(),
                LastConnection = profileResponse.GetData().GetLastConnection(),
                Team           = teamProfileResponse.GetData()
            };
        }
    }
}