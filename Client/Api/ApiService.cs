using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Client.Api.Requests;
using Client.Api.Responses;
using Newtonsoft.Json;

namespace Client.Api
{
    internal class ApiService
    {
        public const string GLOBAL_AUTH   = "/global/auth";
        public const string SCHOOL_DATA   = "/global/school/info";
        public const string QUOTATION     = "/global/quotation";
        public const string COLOR_SCHEMAS = "/global/color/themes";
        public const string CONFIGURATION = "/global/client/config";
        public const string USER_PROFILE  = "/user/profile";
        public const string USER_LOGOUT   = "/user/session/delete";
        public const string TEAM_PROFILE  = "/user/team/profile";
        public const string PREFERENCES   = "/user/preferences";

        public delegate void RequestCallback<T>(HttpStatusCode statusCode, T response) where T : Response;

        private static HttpClient s_HttpClient;
        private static bool s_Initialized;

        public static void Initialize()
        {
            if (s_Initialized)
                return;

            s_HttpClient = new HttpClient
            {
                BaseAddress = new Uri(Configuration.EndpointAddress),
                Timeout = TimeSpan.FromSeconds(10)
            };

            s_Initialized = true;
        }

        private static async Task SendRequestAsync<T>(string uri, HttpMethod method, Dictionary<string, string> headers, StringContent body, RequestCallback<T> callback) where T : Response, new()
        {
#if METRICS_ANALYSIS
            long startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Platform.Log($"Sending {method.Method} request: {uri}");
#endif
            using HttpRequestMessage request = new HttpRequestMessage(method, uri);
            HttpResponseMessage response = new HttpResponseMessage();

            if (body != null)
                request.Content = body;

            if (headers != null)
            {
                for (int i = 0; i < headers.Count; i++)
                    request.Headers.Add(headers.ElementAt(i).Key, headers.ElementAt(i).Value);
            }

#if METRICS_ANALYSIS
            long reqStartTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
#endif

            try
            {
                response = await s_HttpClient.SendAsync(request);
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

#if METRICS_ANALYSIS
            long reqEndTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
#endif

            string requestBody = await response.Content.ReadAsStringAsync();
            T requestData = new T();

            if (!string.IsNullOrWhiteSpace(requestBody))
                requestData = JsonConvert.DeserializeObject<T>(requestBody);

            if (callback != null)
                callback(response.StatusCode, requestData);

#if METRICS_ANALYSIS
            long endTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            long overAll = endTime - startTime;
            long reqProcess = reqEndTime - reqStartTime;
            long methodProcess = overAll - reqProcess;
            Platform.Log($"Received {response.StatusCode}: {uri}\nOverall: {overAll}ms\nRequest Processing: {reqProcess}ms\nCallback Processing: {methodProcess}ms\n");
#endif
        }
        
        public static async Task GetSchoolDataAsync(RequestCallback<SchoolResponse> callback)
        {
            await SendRequestAsync(SCHOOL_DATA, HttpMethod.Get, null, null, callback);
        }

        public static async Task AuthenticateAsync(AuthRequest request, RequestCallback<AuthResponse> callback)
        {
            StringContent requestBody = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            await SendRequestAsync(GLOBAL_AUTH, HttpMethod.Post, null, requestBody, callback);
        }

        public static async Task GetQuotationAsync(RequestCallback<QuotationResponse> callback)
        {
            await SendRequestAsync(QUOTATION, HttpMethod.Get, null, null, callback);
        }

        public static async Task GetProfileAsync(long sessionId, string authToken, RequestCallback<ProfileResponse> callback)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "sessionId", sessionId.ToString() },
                { "authToken", authToken }
            };

            await SendRequestAsync(USER_PROFILE, HttpMethod.Get, headers, null, callback);
        }

        public static async Task LogoutAsync(long sessionId, string authToken, RequestCallback<Response> callback)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "sessionId", sessionId.ToString() },
                { "authToken", authToken }
            };

            await SendRequestAsync(USER_LOGOUT, HttpMethod.Delete, headers, null, callback);
        }

        public static async Task GetThemeList(RequestCallback<ThemeResponse> callback)
        {
            await SendRequestAsync(COLOR_SCHEMAS, HttpMethod.Get, null, null, callback);
        }

        public static async Task GetTeamProfileAsync(long sessionId, string authToken, RequestCallback<TeamProfileResponse> callback)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "sessionId", sessionId.ToString() },
                { "authToken", authToken }
            };

            await SendRequestAsync(TEAM_PROFILE, HttpMethod.Get, headers, null, callback);
        }

        public static async Task GetConfigurationAsync(RequestCallback<ConfigResponse> callback)
        {
            await SendRequestAsync(CONFIGURATION, HttpMethod.Get, null, null, callback);
        }

        public static async Task GetPreferencesAsync(long sessionId, string authToken, RequestCallback<PreferenceResponse> callback)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "sessionId", sessionId.ToString() },
                { "authToken", authToken }
            };

            await SendRequestAsync(PREFERENCES, HttpMethod.Get, headers, null, callback);
        }

        public static async Task UpdatePreferencesAsync(long sessionId, string authToken, PreferenceRequest request, RequestCallback<Response> callback)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "sessionId", sessionId.ToString() },
                { "authToken", authToken }
            };

            StringContent requestBody = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            await SendRequestAsync(PREFERENCES, HttpMethod.Post, headers, requestBody, callback);
        }
    }
}