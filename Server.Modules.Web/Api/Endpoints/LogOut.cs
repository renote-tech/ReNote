using System.Threading.Tasks;
using Server.ReNote.Management;
using Server.Web.Api;
using Server.Web.Utilities;

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
                    return await ApiUtil.SendAsync(405, ApiMessages.MethodNotAllowed());
            }
        }

        /// <summary>
        /// Operates a DELETE request.
        /// </summary>
        /// <param name="req">The <see cref="ApiRequest"/> to be proceeded.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        private static async Task<ApiResponse> Delete(ApiRequest req)
        {
            ApiResponse verification = await ApiUtil.VerifyAuthorizationAsync(req.Headers);
            if (verification.Status != 200)
                return verification;

            long sessionId = long.Parse(ApiUtil.GetHeaderValue(req.Headers["sessionId"]));
            SessionManager.DeleteSession(sessionId);

            return await ApiUtil.SendAsync(200, ApiMessages.Success());
        }
    }
}