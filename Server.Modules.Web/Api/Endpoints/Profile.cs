using Server.Web.Api;
using Server.Web.Utilities;

namespace Server.ReNote.Api
{
    public class Profile
    {
        public static async Task<ApiResponse> OperateRequest(ApiRequest req)
        {
            switch (req.Method.ToUpper())
            {
                case "GET":
                    return await Get(req);
                case "POST":
                    return await Post(req);
                default:
                    return await ApiUtil.SendAsync(405, ApiMessages.MethodNotAllowed());
            }
        }

        private static async Task<ApiResponse> Get(ApiRequest req)
        {
            ApiResponse verificationStatus = await ApiUtil.VerifyAuthorization(req.Headers);
            if (verificationStatus.Status != 200)
                return verificationStatus;



            return await ApiUtil.SendAsync(200, ApiMessages.Success());
        }

        private static async Task<ApiResponse> Post(ApiRequest req)
        {
            return await ApiUtil.SendAsync(200, ApiMessages.Success());
        }
    }
}