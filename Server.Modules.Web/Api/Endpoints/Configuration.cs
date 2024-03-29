﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Server.Common.Helpers;
using Server.ReNote.Data;
using Server.ReNote.Helpers;
using Server.Web.Api;
using Server.Web.Api.Responses;
using Server.Web.Helpers;

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
                    return await ApiHelper.SendAsync(405, ApiMessages.MethodNotAllowed());
            }
        }

        /// <summary>
        /// Operates a GET request.
        /// </summary>
        /// <param name="req">The <see cref="ApiRequest"/> to be proceeded.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        private static async Task<ApiResponse> Get(ApiRequest req)
        {
            string rawMenuItems = DatabaseHelper.Get(Constants.DB_ROOT_CONFIGS, "menu");
            MenuInfo[] menuItems = Array.Empty<MenuInfo>();
            if (JsonHelper.ValiditateJson(rawMenuItems))
                menuItems = JsonConvert.DeserializeObject<MenuInfo[]>(rawMenuItems);

            string rawToolbars = DatabaseHelper.Get(Constants.DB_ROOT_CONFIGS, "toolbars");
            ToolbarInfo[] toolbars = Array.Empty<ToolbarInfo>();
            if (JsonHelper.ValiditateJson(rawToolbars))
                toolbars = JsonConvert.DeserializeObject<ToolbarInfo[]>(rawToolbars);

            string[] rawThemes = DatabaseHelper.GetValues(Constants.DB_ROOT_COLOR_THEMES);
            Theme[] themes = new Theme[rawThemes.Length];
            for (int i = 0; i < themes.Length; i++)
            {
                if (!JsonHelper.ValiditateJson(rawThemes[i]))
                    continue;

                themes[i] = JsonConvert.DeserializeObject<Theme>(rawThemes[i]);
            }

            ConfigResponse response = new ConfigResponse()
            {
                Features     = PluginManager.Features   ,
                MenuInfo     = menuItems,
                ToolbarsInfo = toolbars,
                Themes       = themes
            };

            return await ApiHelper.SendAsync(200, ApiMessages.Success(), response);
        }
    }
}