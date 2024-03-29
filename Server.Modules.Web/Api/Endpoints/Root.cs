﻿using System.Threading.Tasks;
using Server.Web.Api;
using Server.Web.Helpers;

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
            return await ApiHelper.SendAsync(200, ApiMessages.ServerRunning());
        }
    }
}