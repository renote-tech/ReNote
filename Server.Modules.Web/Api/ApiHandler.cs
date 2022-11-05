using System.Net;
using System.Text;

using Server.Common.Utilities;
using Server.Web.Utilities;

namespace Server.Web.Api
{
    internal class ApiHandler
    {
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
                    apiResponse = await ApiData.CallEndpointAsync(apiEndpoint, apiRequest);
                else
                    apiResponse = ApiUtil.SendBasic(404, "Not found");

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
    }
}