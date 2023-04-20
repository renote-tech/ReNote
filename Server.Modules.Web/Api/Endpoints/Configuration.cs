using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Server.ReNote.Data;
using Server.ReNote.Utilities;
using Server.Web.Api;
using Server.Web.Api.Responses;
using Server.Web.Utilities;

namespace Server.ReNote.Api
{
    internal class Configuration
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
            string rawFeatures = DatabaseUtil.Get(Constants.DB_ROOT_CONFIGS, "plugins");
            Dictionary<string, bool> features = JsonConvert.DeserializeObject<Dictionary<string, bool>>(rawFeatures);

            string rawToolbars = DatabaseUtil.Get(Constants.DB_ROOT_CONFIGS, "toolbars");
            ToolbarInfo[] toolbars = JsonConvert.DeserializeObject<ToolbarInfo[]>(rawToolbars);

            ConfigResponse response = new ConfigResponse()
            {
                Features = features,
                ToolbarsInfo = toolbars
            };

            return await ApiUtil.SendWithDataAsync(200, ApiMessages.Success(), response);
        }
    }
}