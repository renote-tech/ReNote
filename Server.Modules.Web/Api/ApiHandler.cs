using System;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Server.Common;
using Server.Web.Utilities;

namespace Server.Web.Api
{
    internal class ApiHandler
    {
        /// <summary>
        /// Handles an incoming API request.
        /// </summary>
        public static async Task Handle()
        {
            while(ApiInterface.Instance.IsRunning)
            {
                HttpListener listener = ApiInterface.Instance.Listener;
                HttpListenerContext apiContext = await listener.GetContextAsync();

#if PROD_SIM && !METRICS_ANALYSIS
                await Task.Delay(new Random().Next(100, 500));
#endif

#if METRICS_ANALYSIS
                long startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
#endif

                ApiResponse apiResponse;
                ApiEndpoint apiEndpoint = ApiAtlas.GetEndpoint(apiContext.Request.RawUrl);
                if (apiEndpoint == null)
                    continue;

                ApiRequest apiRequest = new ApiRequest()
                {
                    Method  = apiContext.Request.HttpMethod,
                    Headers = apiContext.Request.Headers,
                    Query   = apiContext.Request.QueryString,
                    Body    = apiContext.Request.InputStream
                };

#if METRICS_ANALYSIS
                long endpointStartTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
#endif

                if (apiEndpoint != null)
                    apiResponse = await CallEndpointAsync(apiEndpoint, apiRequest);
                else
                    apiResponse = await ApiUtil.SendAsync(404, "Not found");

#if METRICS_ANALYSIS
                long endpointEndTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
#endif

                byte[] body = Array.Empty<byte>();

                if(!string.IsNullOrWhiteSpace(apiResponse.Body))
                    body = Encoding.UTF8.GetBytes(apiResponse.Body);

                apiContext.Request.InputStream.Close();

                apiContext.Response.StatusCode = apiResponse.Status;
                apiContext.Response.ContentType = "application/json";

                apiContext.Response.Headers.Add("Server", string.Empty);
                apiContext.Response.Headers.Add("Server-Agent", ServerInfo.Agent);
                apiContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                if (apiRequest.Method == "OPTIONS")
                {
                    apiContext.Response.Headers.Add("Access-Control-Allow-Headers", "*");
                    apiContext.Response.Headers.Add("Access-Control-Allow-Methods", "*");
                }

                await apiContext.Response.OutputStream.WriteAsync(body);
                apiContext.Response.KeepAlive = false;

                try
                {
                    apiContext.Response.OutputStream.Close();
                    apiContext.Response.Close();
                } catch(Exception ex) when (ex is HttpListenerException)
                {
                    Platform.Log(ex.ToString(), LogLevel.ERROR);
                }

#if METRICS_ANALYSIS
                long endTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                Platform.Log($"Sent {(HttpStatusCode)apiContext.Response.StatusCode}: {apiEndpoint.Uri}\nOverall: {endTime - startTime}ms\nEndpoint Processing: {endpointEndTime - endpointStartTime}ms\n");
#endif
            }
        }

        /// <summary>
        /// Calls an <see cref="ApiEndpoint"/>'s OperateRequest method.
        /// </summary>
        /// <param name="endpoint">The targeted <see cref="ApiEndpoint"/>.</param>
        /// <param name="request">The <see cref="ApiRequest"/> to be proceeded.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        private static async Task<ApiResponse> CallEndpointAsync(ApiEndpoint endpoint, ApiRequest request)
        {
            Type endpointClass = Type.GetType($"Server.ReNote.Api.{endpoint.Name}");

            if (endpointClass == null)
                return await ApiUtil.SendErrorAsync(ApiMessages.EndpointNotFound());

            MethodInfo endpointMethod = endpointClass.GetMethod("OperateRequest", BindingFlags.Public | BindingFlags.Static);

            if (endpointMethod == null)
                return await ApiUtil.SendErrorAsync(ApiMessages.EndpointMethodNotFound());

            return await (Task<ApiResponse>)endpointMethod.Invoke(null, new object[] { request });
        }
    }
}