using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Server.Common.Helpers;
using Server.ReNote.Helpers;
using Server.Web.Api;
using Server.Web.Api.Requests;
using Server.Web.Api.Responses;
using Server.Web.Helpers;

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
                case "PATCH":
                    return await Patch(req);
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
            VerificationResponse verification = await ApiHelper.VerifyAuthorizationAsync(req.Headers);
            if (verification.Response.Status != 200)
                return verification.Response;


            PreferenceResponse response = DatabaseHelper.GetAs<PreferenceResponse>(Constants.DB_ROOT_PREFERENCES, verification.UserId.ToString());
            response ??= new PreferenceResponse() { Language = null, Theme = null };

            return await ApiHelper.SendAsync(200, ApiMessages.Success(), response);
        }

        /// <summary>
        /// Operates a PATCH request.
        /// </summary>
        /// <param name="req">The <see cref="ApiRequest"/> to be proceeded.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        private static async Task<ApiResponse> Patch(ApiRequest req)
        {
            VerificationResponse verification = await ApiHelper.VerifyAuthorizationAsync(req.Headers);
            if (verification.Response.Status != 200)
                return verification.Response;

            string requestBody = await StreamHelper.GetStringAsync(req.Body);
            if (string.IsNullOrWhiteSpace(requestBody))
                return await ApiHelper.SendAsync(400, ApiMessages.EmptyLanguageOrTheme());

            if (!JsonHelper.ValiditateJson(requestBody))
                return await ApiHelper.SendAsync(400, ApiMessages.InvalidJson());

            PreferenceRequest requestData = JsonConvert.DeserializeObject<PreferenceRequest>(requestBody);
            PreferenceResponse preferences = DatabaseHelper.GetAs<PreferenceResponse>(Constants.DB_ROOT_PREFERENCES, verification.UserId.ToString());

            DatabaseHelper.Set(Constants.DB_ROOT_PREFERENCES, verification.UserId.ToString(), new PreferenceRequest()
            {
                Language = string.IsNullOrWhiteSpace(requestData.Language) ? preferences.Language : requestData.Language,
                Theme    = string.IsNullOrWhiteSpace(requestData.Theme) ? preferences.Theme: requestData.Theme
            });

            return await ApiHelper.SendAsync(200, ApiMessages.Success());
        }
    }
}