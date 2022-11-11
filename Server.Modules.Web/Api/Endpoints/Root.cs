using Server.Web.Api;
using Server.Web.Utilities;

namespace Server.ReNote.Api
{
    public class Root
    {
        public static async Task<ApiResponse> OperateRequest(ApiRequest req)
        {
            return await ApiUtil.SendAsync(200, "Server is running");
        }
    }
}