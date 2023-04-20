using System.Threading.Tasks;
using Server.ReNote.Data;
using Server.ReNote.Utilities;
using Server.Web.Api;
using Server.Web.Utilities;
using Newtonsoft.Json;

namespace Server.ReNote.Api 
{
    internal class ColorTheme
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
            string[] rawThemes = DatabaseUtil.GetValues(Constants.DB_ROOT_COLOR_THEMES);
            Theme[] themes = new Theme[rawThemes.Length];
            for (int i = 0; i < themes.Length; i++)
                themes[i] = JsonConvert.DeserializeObject<Theme>(rawThemes[i]);

            return await ApiUtil.SendWithDataAsync(200, ApiMessages.Success(), themes);
        }
    }
}