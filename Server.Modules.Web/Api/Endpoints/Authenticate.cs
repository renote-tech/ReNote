using Server.Common.Encryption;
using Server.Common.Utilities;
using Server.ReNote.Data;
using Server.ReNote.Management;
using Server.Web.Api;
using Server.Web.Api.Requests;
using Server.Web.Api.Responses;
using Server.Web.Utilities;

namespace Server.ReNote.Api
{
    public class Authenticate
    {
        /// <summary>
        /// Operates a request.
        /// </summary>
        /// <param name="req">The <see cref="ApiRequest"/> to be proceeded.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public static async Task<ApiResponse> OperateRequest(ApiRequest req)
        {
            switch(req.Method.ToUpper())
            {
                case "POST":
                    return await Post(req);
                default:
                    return await ApiUtil.SendAsync(405, ApiMessages.MethodNotAllowed());
            }
        }

        /// <summary>
        /// Operates a POST request.
        /// </summary>
        /// <param name="req">The <see cref="ApiRequest"/> to be proceeded.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        private static async Task<ApiResponse> Post(ApiRequest req)
        {
            AuthRequest reqBody = await StreamUtil.Convert<AuthRequest>(req.Body);

            if (reqBody == null)
                return await ApiUtil.SendAsync(400, ApiMessages.InvalidJson());

            if(string.IsNullOrWhiteSpace(reqBody.Username) || string.IsNullOrWhiteSpace(reqBody.Password))
                return await ApiUtil.SendAsync(400, ApiMessages.NullOrEmpty("username", "password"));

            if (UserManager.GetUserId(reqBody.Username) == -1)
                return await ApiUtil.SendAsync(400, ApiMessages.UserNotExists());

            User userData = UserManager.GetUser(reqBody.Username);
            byte[] hashPassword = await EncryptionUtil.ComputeSha256Async(reqBody.Password);
            AESObject aesObject = new AESObject(userData.SecurePassword, iv: userData.IVPassword, key: hashPassword);

            if (AES.Decrypt(aesObject) == string.Empty)
                return await ApiUtil.SendAsync(401, ApiMessages.InvalidPassword());

            GlobalSession session = await SessionManager.CreateSessionAsync(userData.UserId);
            AuthResponse response = new AuthResponse()
            {
                SessionId   = session.SessionId,
                UserId      = session.UserId,
                AccountType = session.AccountType,
                AuthToken   = session.AuthToken
            };

            return await ApiUtil.SendWithDataAsync(200, ApiMessages.Success(), response);
        }
    }
}