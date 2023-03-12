using Server.Web.Api;
using Server.Web.Api.Responses;
using Server.Web.Utilities;

namespace Server.ReNote.Api
{
    public class AboutEndpoint
    {
        /// <summary>
        /// Operates a request.
        /// </summary>
        /// <param name="req">The <see cref="ApiRequest"/> to be proceeded.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public static async Task<ApiResponse> OperateRequest(ApiRequest req)
        {
            switch (req.Method.ToUpper())
            {
                case "GET":
                    return await Get(req);
                default:
                    return await ApiUtil.SendAsync(405, ApiMessages.MethodNotAllowed());
            }
        }

        /// <summary>
        /// Operates a GET request.
        /// </summary>
        /// <param name="req">The <see cref="ApiRequest"/> to be proceeded.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        private static async Task<ApiResponse> Get(ApiRequest req)
        {
            AboutResponse response = new AboutResponse()
            {
                SoftwareName      = "ReNote Σ/Ω",
                SoftwareVersion   = "ReNote 2022.11",
                SoftwareCopyright = "© ReNote NETW. All rights reserved."
            };

            return await ApiUtil.SendWithDataAsync(200, ApiMessages.Success(), response);
        }
    }
}