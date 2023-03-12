using Newtonsoft.Json;
using Server.ReNote.Data;
using Server.ReNote.Management;
using Server.Web.Api;
using Server.Web.Api.Responses;
using Server.Web.Utilities;

namespace Server.ReNote.Api
{
    public class ProfileEndpoint
    {
        /// <summary>
        /// Operates a request.
        /// </summary>
        /// <param name="req">The <see cref="ApiRequest"/> to be proceeded.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public static async Task<ApiResponse> OperateRequest(ApiRequest req)
        {
            switch (req.Method.ToUpper())
            {
                case "GET":
                    return await Get(req);
                default:
                    return await ApiUtil.SendAsync(405, ApiMessages.MethodNotAllowed());
            }
        }

        /// <summary>
        /// Operates a GET request.
        /// </summary>
        /// <param name="req">The <see cref="ApiRequest"/> to be proceeded.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        private static async Task<ApiResponse> Get(ApiRequest req)
        {
            ApiResponse verification = await ApiUtil.VerifyAuthorizationAsync(req.Headers);
            if (verification.Status != 200)
                return verification;

            DataResponse verificationResponse = JsonConvert.DeserializeObject<DataResponse>(verification.Body);
            long userId = (long)verificationResponse.Data;

            User userData = UserManager.GetUser(userId);
            ProfileResponse response = new ProfileResponse()
            {
                RealName       = userData.RealName,
                ProfilePicture = userData.ProfilePicture,
                Birthday       = userData.Birthday,
                Email          = userData.Email,
                LastConnection = userData.LastConnection,
                Phone          = userData.Phone
            };

            return await ApiUtil.SendWithDataAsync(200, ApiMessages.Success(), response);
        }
    }
}