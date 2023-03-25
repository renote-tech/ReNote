﻿using System.Dynamic;
using System.Net;
using Client.Utilities;

namespace Client.Api
{
    public class ApiClient
    {
        public const string GLOBAL_AUTH  = "/global/v1/auth";
        public const string SCHOOL_DATA  = "/global/v1/school/info";
        public const string QUOTATION    = "/global/v1/quotation";
        public const string USER_PROFILE = "/user/v1/profile";
        public const string USER_LOGOUT  = "/user/v1/session/delete";

        private static HttpClient httpClient;
        private static bool initialized;

        public static void Initialize()
        {
            if (initialized)
                return;

            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://127.0.0.1:7101");
            initialized = true;
        }

        public static async Task<HttpResponseMessage> SendRequestAsync(string uri, HttpMethod method, Dictionary<string, string> headers, StringContent body)
        {
            Platform.Log($"Sending {method.Method} request: {uri}");
            using HttpRequestMessage request = new HttpRequestMessage(method, uri);
            HttpResponseMessage response = new HttpResponseMessage();

            if (body != null)
                request.Content = body;

            if (headers != null)
            {
                for (int i = 0; i < headers.Count; i++)
                    request.Headers.Add(headers.ElementAt(i).Key, headers.ElementAt(i).Value);
            }

            try
            { response = await httpClient.SendAsync(request); }
            catch (HttpRequestException)
            { response.StatusCode = HttpStatusCode.InternalServerError; }

            Platform.Log($"Received {response.StatusCode}: {uri}");

            return response;
        }
        
        public static async Task<HttpResponseMessage> GetSchoolDataAsync()
        {
            return await SendRequestAsync(SCHOOL_DATA, HttpMethod.Get, null, null);
        }

        public static async Task<HttpResponseMessage> AuthenticateAsync(string username, string password)
        {
            dynamic obj = new ExpandoObject();
            obj.username = username;
            obj.password = password;
            return await SendRequestAsync(GLOBAL_AUTH, HttpMethod.Post, null, JsonUtil.SerializeAsBody(obj));
        }

        public static async Task<HttpResponseMessage> GetQuotationAsync()
        {
            return await SendRequestAsync(QUOTATION, HttpMethod.Get, null, null);
        }

        public static async Task<HttpResponseMessage> GetProfileAsync(long sessionId, string authToken)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "sessionId", sessionId.ToString() },
                { "authToken", authToken }
            };

            return await SendRequestAsync(USER_PROFILE, HttpMethod.Get, headers, null);
        }

        public static async Task<HttpResponseMessage> SendLogoutAsync(long sessionId, string authToken)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "sessionId", sessionId.ToString() },
                { "authToken", authToken }
            };

            return await SendRequestAsync(USER_LOGOUT, HttpMethod.Delete, headers, null);
        }
    }
}