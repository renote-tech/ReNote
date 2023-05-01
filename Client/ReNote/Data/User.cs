using System.Net;
using System.Threading.Tasks;
using Client.Api;
using Client.Api.Responses;

namespace Client.ReNote.Data
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
            Current = new User()
            {
                UserId = data.UserId,
                SessionId = data.SessionId,
                AccountType = data.AccountType,
                AuthToken = data.AuthToken
            };

            bool hasErrorResponse = false;

            await ApiService.GetProfileAsync((statusCode, response) =>
            {
                if (statusCode != HttpStatusCode.OK)
                {
                    hasErrorResponse = true;
                    return;
                }

                Current.RealName = response.Data.RealName;
                Current.TeamId = response.Data.TeamId;
                Current.ProfilePicture = response.Data.ProfilePicture;
                Current.Email = response.Data.Email;
                Current.PhoneNumber = response.Data.PhoneNumber;
                Current.Birthday = response.Data.Birthday;
                Current.LastConnection = response.Data.LastConnection;
            });

            await ApiService.GetTeamProfileAsync((statusCode, response) =>
            {
                if (hasErrorResponse || statusCode != HttpStatusCode.OK)
                    return;

                Current.Team = response.Data;
            });

            await ApiService.GetPreferencesAsync((statusCode, response) =>
            {
                if (hasErrorResponse || statusCode != HttpStatusCode.OK)
                    return;

                Current.Language = response.Data.Language;
                Current.Theme = response.Data.Theme;
            });

            if (hasErrorResponse)
                Current = null;
        }

        public static void Delete()
        {
            Current = null;
        }
    }
}