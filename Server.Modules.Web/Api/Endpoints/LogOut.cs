using System.Threading.Tasks;
using Server.ReNote.Management;
using Server.Web.Api;
using Server.Web.Api.Responses;
using Server.Web.Helpers;

namespace Server.ReNote.Api
{
    internal class LogOut
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
                case "DELETE":
                    return await Delete(req);
                default:
                    return await ApiHelper.SendAsync(405, ApiMessages.MethodNotAllowed());
            }
        }

        /// <summary>
        /// Operates a DELETE request.
        /// </summary>
        /// <param name="req">The <see cref="ApiRequest"/> to be proceeded.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        private static async Task<ApiResponse> Delete(ApiRequest req)
        {
            VerificationResponse verification = await ApiHelper.VerifyAuthorizationAsync(req.Headers);
            if (verification.Response.Status != 200)
                return verification.Response;

            SessionManager.DeleteSession(verification.SessionId);

            return await ApiHelper.SendAsync(200, ApiMessages.Success());
        }
    }
}