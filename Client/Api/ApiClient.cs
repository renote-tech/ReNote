using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Dynamic;

using Client.Api.Requests;
using Client.Utilities;

namespace Client.Api
{
    public class ApiClient
    {
        public static ApiClient Instance
        {
            get
            {
                instance ??= new ApiClient();
                return instance;
            }
            set
            {
                instance = value;
            }
        }

        private static ApiClient instance;
        private HttpClient httpClient;

        public ApiClient()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:7101");
        }

        public async Task<HttpResponseMessage> SendRequest(string uri, HttpMethod method, StringContent body, Dictionary<string, string> headers)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(method, uri))
            {
                HttpResponseMessage response = new HttpResponseMessage();

                if (body != null)
                    request.Content = body;

                if (headers != null)
                {
                    for (int i = 0; i < headers.Count; i++)
                        request.Headers.Add(headers.ElementAt(i).Key, headers.ElementAt(i).Value);
                }

                try
                {
                    response = await httpClient.SendAsync(request);
                }
                catch (HttpRequestException)
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                }
                return response;
            }
        }

        public async Task<HttpResponseMessage> SendAuth(string username, string password)
        {
            dynamic obj = new ExpandoObject();
            obj.username = username;
            obj.password = password;
            return await SendRequest(Endpoints.GLOBAL_AUTH, HttpMethod.Post, JsonUtil.SerializeAsBody(obj), null);
        }
    }
}