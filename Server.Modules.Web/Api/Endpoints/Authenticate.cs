using Server.Web.Api;
using Server.Web.Utilities;

namespace Server.ReNote.Api
{
    public class Authenticate
    {
        public static async Task<ApiResponse> OperateRequest(ApiRequest req)
        {
            switch(req.Method.ToUpper())
            {
                case "POST":
                    return await Post(req);
                default:
                    return ApiUtil.SendBasic(405, ApiMessages.MethodNotAllowed());
            }
        }


        private static async Task<ApiResponse> Post(ApiRequest req)
        {
            dynamic body = await StreamUtil.ConvertToDynamic(req.Body);

            if(!DynamicUtil.HasProperty(body, "username") || !DynamicUtil.HasProperty(body, "password"))
                return ApiUtil.SendBasic(400, ApiMessages.NullOrEmpty("username", "password"));

            if(string.IsNullOrWhiteSpace(body.username) || string.IsNullOrWhiteSpace(body.password))
                return ApiUtil.SendBasic(400, ApiMessages.NullOrEmpty("username", "password"));

            return ApiUtil.SendBasic(200, $"Success");
        }
    }
}