using System.Threading.Tasks;
using Server.ReNote.Data;
using Server.ReNote.Management;
using Server.ReNote.Helpers;
using Server.Web.Api;
using Server.Web.Api.Responses;
using Server.Web.Helpers;
using Newtonsoft.Json;

namespace Server.ReNote.Api
{
    internal class TeamProfile
    {
        /// <summary>
        /// Operates a request.
        /// </summary>
        /// <param name="req">The <see cref="ApiRequest"/> to be proceeded.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public static async Task<ApiResponse> OperateRequest(ApiRequest req)
        {
            switch (req.Method)
            {
                case "GET":
                    return await Get(req);
                default:
                    return await ApiHelper.SendAsync(405, ApiMessages.MethodNotAllowed());
            }
        }

        /// <summary>
        /// Operates a GET request.
        /// </summary>
        /// <param name="req">The <see cref="ApiRequest"/> to be proceeded.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        private static async Task<ApiResponse> Get(ApiRequest req)
        {
            ApiResponse verification = await ApiHelper.VerifyAuthorizationAsync(req.Headers);
            if (verification.Status != 200)
                return verification;

            DataResponse verificationResponse = JsonConvert.DeserializeObject<DataResponse>(verification.Body);
            long userId = (long)verificationResponse.Data;

            User userData = UserManager.GetUser(userId);
            Team teamData = DatabaseHelper.GetAs<Team>(Constants.DB_ROOT_TEAMS, userData.TeamId.ToString());

            TeamProfileResponse response = new TeamProfileResponse()
            {
                TeamName     = teamData.TeamName,
                TeamId       = teamData.TeamId,
                TeamGrade    = teamData.TeamGrade,
                TeamLeaderId = teamData.TeamLeaderId,
                Delegates    = teamData.Delegates,
                Alternates   = teamData.Alternates
            };

            return await ApiHelper.SendAsync(200, ApiMessages.Success(), response);
        }
    }
}