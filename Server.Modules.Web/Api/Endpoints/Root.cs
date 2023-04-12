using System.Threading.Tasks;
using Server.Web.Api;
using Server.Web.Utilities;

namespace Server.ReNote.Api
{
    internal class Root
    {
        /// <summary>
        /// Operates a request.
        /// </summary>
        /// <param name="req">The <see cref="ApiRequest"/> to be proceeded.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public static async Task<ApiResponse> OperateRequest(ApiRequest req)
        {
            return await ApiUtil.SendAsync(200, "Server is running");
        }
    }
}