using System.Net;
using System.Threading.Tasks;
using Client.Api;
using Client.Api.Responses;
using Client.Managers;

namespace Client.ReNote
{
    public class User
    {
        public static User Current { get; private set; }

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
        public string Language { get; set; }
        public string Theme { get; set; }

        public static async Task GetAsync(AuthData data)
        {
            ProfileResponse profileResponse = null;
            TeamProfileResponse teamProfileResponse = null;
            PreferenceResponse preferenceResponse = null;

            await ApiService.GetProfileAsync(data.SessionId, data.AuthToken, (HttpStatusCode statusCode, ProfileResponse response) =>
            {
                if (statusCode != HttpStatusCode.OK)
                    return;

                profileResponse = response;
            });

            await ApiService.GetTeamProfileAsync(data.SessionId, data.AuthToken, (HttpStatusCode statusCode, TeamProfileResponse response) =>
            {
                if (statusCode != HttpStatusCode.OK)
                    return;

                teamProfileResponse = response;
            });

            await ApiService.GetPreferencesAsync(data.SessionId, data.AuthToken, (HttpStatusCode statusCode, PreferenceResponse response) =>
            {
                if (statusCode != HttpStatusCode.OK)
                    return;

                preferenceResponse = response;
            });

            if (profileResponse == null || teamProfileResponse == null || preferenceResponse == null)
            {
                Current = null;
                return;
            }

            Current = new User()
            {
                UserId         = data.UserId,
                SessionId      = data.SessionId,
                AccountType    = data.AccountType,
                AuthToken      = data.AuthToken,
                RealName       = profileResponse.Data.RealName,
                TeamId         = profileResponse.Data.TeamId,
                ProfilePicture = profileResponse.Data.ProfilePicture,
                Email          = profileResponse.Data.Email,
                PhoneNumber    = profileResponse.Data.PhoneNumber,
                Birthday       = profileResponse.Data.Birthday,
                LastConnection = profileResponse.Data.LastConnection,
                Team           = teamProfileResponse.Data,
                Language       = preferenceResponse.Data.Language,
                Theme          = preferenceResponse.Data.Theme
            };
        }

        public static void Delete()
        {
            Current = null;
        }
    }
}