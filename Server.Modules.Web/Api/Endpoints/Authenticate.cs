using System.Threading.Tasks;
using Server.Common.Encryption;
using Server.Common.Helpers;
using Server.ReNote.Data;
using Server.ReNote.Management;
using Server.Web.Api;
using Server.Web.Api.Requests;
using Server.Web.Api.Responses;
using Server.Web.Helpers;
using Newtonsoft.Json;

namespace Server.ReNote.Api
{
    internal class Authenticate
    {
        /// <summary>
        /// Operates a request.
        /// </summary>
        /// <param name="req">The <see cref="ApiRequest"/> to be proceeded.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public static async Task<ApiResponse> OperateRequest(ApiRequest req)
        {
            switch(req.Method)
            {
                case "POST":
                    return await Post(req);
                default:
                    return await ApiHelper.SendAsync(405, ApiMessages.MethodNotAllowed());
            }
        }

        /// <summary>
        /// Operates a POST request.
        /// </summary>
        /// <param name="req">The <see cref="ApiRequest"/> to be proceeded.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        private static async Task<ApiResponse> Post(ApiRequest req)
        {
            string requestBody = await StreamHelper.GetStringAsync(req.Body);
            if (string.IsNullOrWhiteSpace(requestBody))
                return await ApiHelper.SendAsync(400, ApiMessages.EmptyUsernameOrPassword());

            if (!JsonHelper.ValiditateJson(requestBody))
                return await ApiHelper.SendAsync(400, ApiMessages.InvalidJson());

            AuthRequest requestData = JsonConvert.DeserializeObject<AuthRequest>(requestBody);

            if(string.IsNullOrWhiteSpace(requestData.Username) || string.IsNullOrWhiteSpace(requestData.Password))
                return await ApiHelper.SendAsync(400, ApiMessages.EmptyUsernameOrPassword());

            if (UserManager.GetUserId(requestData.Username) == -1)
                return await ApiHelper.SendAsync(400, ApiMessages.InvalidUsernameOrPassword());

            User userData = UserManager.GetUser(requestData.Username);
            byte[] hashPassword = await Sha256.ComputeAsync(requestData.Password);
            AESObject aesObject = new AESObject(userData.SecurePassword, iv: userData.IVPassword, key: hashPassword);

            if (!AES.VerifyKey(requestData.Password, aesObject))
                return await ApiHelper.SendAsync(401, ApiMessages.InvalidUsernameOrPassword());

            Session session = await SessionManager.CreateSessionAsync(userData.UserId);
            AuthResponse response = new AuthResponse()
            {
                SessionId   = session.SessionId,
                UserId      = session.UserId,
                AccountType = session.AccountType,
                AuthToken   = session.AuthToken
            };

            return await ApiHelper.SendAsync(200, ApiMessages.Success(), response);
        }
    }
}