using Newtonsoft.Json;
using Server.Common.Encryption;
using Server.Common.Helpers;
using Server.ReNote.Data;
using Server.ReNote.Helpers;
using Server.ReNote.Management;
using Server.Web.Api;
using Server.Web.Api.Requests;
using Server.Web.Api.Responses;
using Server.Web.Helpers;
using System.Threading.Tasks;

namespace Server.ReNote.Api
{
    internal class Password
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
                case "PATCH":
                    return await Patch(req);
                default:
                    return await ApiHelper.SendAsync(405, ApiMessages.MethodNotAllowed());
            }
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

            User userData = UserManager.GetUser(verification.UserId);

            if (!PluginManager.Enabled(userData.AccountType, Plugins.KN_CHANGE_PASSWORD))
                return await ApiHelper.SendAsync(400, ApiMessages.PluginDisabled());

            string requestBody = await StreamHelper.GetStringAsync(req.Body);
            if (string.IsNullOrWhiteSpace(requestBody))
                return await ApiHelper.SendAsync(400, ApiMessages.EmptyCurrentPasswordOrNewPassword());

            if (!JsonHelper.ValiditateJson(requestBody))
                return await ApiHelper.SendAsync(400, ApiMessages.InvalidJson());

            PasswordRequest requestData = JsonConvert.DeserializeObject<PasswordRequest>(requestBody);

            if (string.IsNullOrWhiteSpace(requestData.CurrentPassword) || string.IsNullOrWhiteSpace(requestData.NewPassword))
                return await ApiHelper.SendAsync(400, ApiMessages.EmptyCurrentPasswordOrNewPassword());

            if (requestData.CurrentPassword == requestData.NewPassword)
                return await ApiHelper.SendAsync(400, ApiMessages.CannotUseSamePassword());

            byte[] hashPassword = await Sha256.ComputeAsync(requestData.CurrentPassword);
            AESObject aesObject = new AESObject(userData.SecurePassword, iv: userData.IVPassword, key: hashPassword);

            if (!AES.VerifyKey(requestData.CurrentPassword, aesObject))
                return await ApiHelper.SendAsync(400, ApiMessages.InvalidPassword());

            AESObject newAesObject = AES.Encrypt(requestData.NewPassword);
            userData.SecurePassword = newAesObject.Data;
            userData.IVPassword = newAesObject.IV;

            DatabaseHelper.Set(Constants.DB_ROOT_USERS, verification.UserId.ToString(), userData);

            SessionManager.DeleteSession(verification.SessionId);

            return await ApiHelper.SendAsync(200, ApiMessages.Success());
        }
    }
}