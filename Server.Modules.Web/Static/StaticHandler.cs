using System.Net;
using Newtonsoft.Json;
using Server.Common;
using Server.ReNote;
using Server.Web.Api;
using Server.Web.Api.Responses;
using Server.Web.Utilities;

namespace Server.Web.Static
{
    internal class StaticHandler
    {
        /// <summary>
        /// Handles an incoming web request.
        /// </summary>
        public static async Task Handle()
        {
            while (StaticInterface.Instance.IsRunning)
            {
                if (StaticInterface.Instance.IsDisposed)
                    return;

                HttpListener listener = StaticInterface.Instance.Listener;
                HttpListenerContext webContext = listener.GetContext();

                byte[] webResponse;
                long userId = Constants.PUBLIC_AUTH_ID;

                if (webContext.Request.RawUrl == null)
                    return;

                ApiResponse authResponse = await ApiUtil.VerifyAuthorizationAsync(webContext.Request.Headers);
                if(authResponse.Status == 200)
                {
                    DataResponse authData = JsonConvert.DeserializeObject<DataResponse>(authResponse.Body);
                    userId = (long)authData.Data;
                }

                webResponse = await StaticStorage.GetResourceAsync(webContext.Request.RawUrl, userId);

                webContext.Response.Headers.Add("Server", string.Empty);
                webContext.Response.Headers.Add("Server-Agent", ServerEnv.Agent);

                webContext.Request.InputStream.Close();
                
                webContext.Response.OutputStream.Write(webResponse, 0, webResponse.Length);
                webContext.Response.KeepAlive = false;
                webContext.Response.OutputStream.Close();
                webContext.Response.Close();
            }
        }
    }
}