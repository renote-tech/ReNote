using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Server.Common.Utilities;
using Server.ReNote.Utilities;
using Server.Web.Api;
using Server.Web.Api.Requests;
using Server.Web.Api.Responses;
using Server.Web.Utilities;

namespace Server.ReNote.Api
{
    internal class Preferences
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
                case "POST":
                    return await Post(req);
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
            string userId = verificationResponse.Data.ToString();

            PreferenceResponse response = DatabaseUtil.GetAs<PreferenceResponse>(Constants.DB_ROOT_PREFERENCES, userId);
            if (response == null)
                response = new PreferenceResponse() { Language = null, Theme = null };

            return await ApiUtil.SendWithDataAsync(200, ApiMessages.Success(), response);
        }

        /// <summary>
        /// Operates a POST request.
        /// </summary>
        /// <param name="req">The <see cref="ApiRequest"/> to be proceeded.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        private static async Task<ApiResponse> Post(ApiRequest req)
        {
            ApiResponse verification = await ApiUtil.VerifyAuthorizationAsync(req.Headers);
            if (verification.Status != 200)
                return verification;

            DataResponse verificationResponse = JsonConvert.DeserializeObject<DataResponse>(verification.Body);
            string userId = verificationResponse.Data.ToString();

            string requestBody = await StreamUtil.GetStringAsync(req.Body);
            if (string.IsNullOrWhiteSpace(requestBody))
                return await ApiUtil.SendAsync(400, ApiMessages.EmptyLanguageOrTheme());

            if (!JsonUtil.ValiditateJson(requestBody))
                return await ApiUtil.SendAsync(400, ApiMessages.InvalidJson());

            PreferenceRequest requestData = JsonConvert.DeserializeObject<PreferenceRequest>(requestBody);
            PreferenceResponse preferences = DatabaseUtil.GetAs<PreferenceResponse>(Constants.DB_ROOT_PREFERENCES, userId);

            DatabaseUtil.Set(Constants.DB_ROOT_PREFERENCES, userId, new PreferenceRequest()
            {
                Language = string.IsNullOrWhiteSpace(requestData.Language) ? preferences.Language : requestData.Language,
                Theme    = string.IsNullOrWhiteSpace(requestData.Theme) ? preferences.Theme: requestData.Theme
            });

            return await ApiUtil.SendAsync(200, ApiMessages.Success());
        }
    }
}