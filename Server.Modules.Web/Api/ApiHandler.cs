using System.Net;
using System.Reflection;
using System.Text;
using Server.Common.Utilities;
using Server.Web.Utilities;

namespace Server.Web.Api
{
    internal class ApiHandler
    {
        /// <summary>
        /// Handles an incoming API request.
        /// </summary>
        public static async void Handle()
        {
            while(ApiInterface.Instance.IsRunning)
            {
                HttpListener listener = ApiInterface.Instance.Listener;
                HttpListenerContext apiContext = await listener.GetContextAsync();

                ApiResponse apiResponse;
                ApiEndpoint apiEndpoint = ApiAtlas.GetEndpoint(apiContext.Request.RawUrl);
                ApiRequest apiRequest = new ApiRequest()
                {
                    Method  = apiContext.Request.HttpMethod,
                    Headers = apiContext.Request.Headers,
                    Query   = apiContext.Request.QueryString,
                    Body    = apiContext.Request.InputStream
                };

                if (apiEndpoint != null)
                    apiResponse = await CallEndpointAsync(apiEndpoint, apiRequest);
                else
                    apiResponse = await ApiUtil.SendAsync(404, "Not found");

                byte[] body = Encoding.UTF8.GetBytes(apiResponse.Body);

                apiContext.Request.InputStream.Close();

                apiContext.Response.StatusCode = apiResponse.Status;
                apiContext.Response.ContentType = "application/json";

                apiContext.Response.Headers.Add("Server", string.Empty);
                apiContext.Response.Headers.Add("Server-Agent", ServerEnvironment.Agent());

                await apiContext.Response.OutputStream.WriteAsync(body);
                apiContext.Response.KeepAlive = false;
                apiContext.Response.OutputStream.Close();
                apiContext.Response.Close();
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
                return await ApiUtil.SendErrorAsync("Endpoint not found");

            MethodInfo endpointMethod = endpointClass.GetMethod("OperateRequest", BindingFlags.Public | BindingFlags.Static);

            if (endpointMethod == null)
                return await ApiUtil.SendErrorAsync("Endpoint call not found");

            Task<ApiResponse> response = (Task<ApiResponse>)endpointMethod.Invoke(null, new object[] { request });
            return await response;
        }
    }
}