using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Client.Api.Requests;
using Client.Api.Responses;
using Client.ReNote.Data;
using Newtonsoft.Json;

namespace Client.Api
{
    internal class ApiService
    {
        private const string GLOBAL_AUTH   = "/global/auth";
        private const string SCHOOL_DATA   = "/global/school/info";
        private const string QUOTATION     = "/global/quotation";
        private const string CONFIGURATION = "/global/client/config";
        private const string USER_PROFILE  = "/user/profile";
        private const string USER_LOGOUT   = "/user/session/delete";
        private const string TEAM_PROFILE  = "/user/team/profile";
        private const string PREFERENCES   = "/user/preferences";

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
                Timeout     = TimeSpan.FromMilliseconds(7500)
            };

            s_Initialized = true;
        }

        private static async Task SendRequestAsync<T>(string uri, HttpMethod method, Request body, RequestCallback<T> callback) where T : Response, new()
        {
#if METRICS_ANALYSIS
            long startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Platform.Log($"Sending {method.Method} request: {uri}");
#endif

            HttpRequestMessage request = new HttpRequestMessage(method, uri);
            HttpResponseMessage response = null;
            
            if (User.Current != null)
            {
                request.Headers.Add("sessionId", User.Current.SessionId.ToString());
                request.Headers.Add("authToken", User.Current.AuthToken);
            }

            if (body != null)
                request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

#if METRICS_ANALYSIS
            long reqStartTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
#endif
            
            try
            {
                response = await s_HttpClient.SendAsync(request);
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                response ??= new HttpResponseMessage();
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

            request.Dispose();
            response.Dispose();
        }

        public static async Task GetSchoolDataAsync(RequestCallback<SchoolResponse> callback)
        {
            await SendRequestAsync(SCHOOL_DATA, HttpMethod.Get, null, callback);
        }

        public static async Task AuthenticateAsync(AuthRequest request, RequestCallback<AuthResponse> callback)
        {
            await SendRequestAsync(GLOBAL_AUTH, HttpMethod.Post, request, callback);
        }

        public static async Task GetQuotationAsync(RequestCallback<QuotationResponse> callback)
        {
            await SendRequestAsync(QUOTATION, HttpMethod.Get, null, callback);
        }

        public static async Task GetProfileAsync(RequestCallback<ProfileResponse> callback)
        {
            await SendRequestAsync(USER_PROFILE, HttpMethod.Get, null, callback);
        }

        public static async Task LogoutAsync(RequestCallback<Response> callback)
        {
            await SendRequestAsync(USER_LOGOUT, HttpMethod.Delete, null, callback);
        }

        public static async Task GetTeamProfileAsync(RequestCallback<TeamProfileResponse> callback)
        {
            await SendRequestAsync(TEAM_PROFILE, HttpMethod.Get, null, callback);
        }

        public static async Task GetConfigurationAsync(RequestCallback<ConfigResponse> callback)
        {
            await SendRequestAsync(CONFIGURATION, HttpMethod.Get, null, callback);
        }

        public static async Task GetPreferencesAsync(RequestCallback<PreferenceResponse> callback)
        {
            await SendRequestAsync(PREFERENCES, HttpMethod.Get, null, callback);
        }

        public static async Task SetPreferencesAsync(PreferenceRequest request, RequestCallback<Response> callback)
        {
            await SendRequestAsync(PREFERENCES, HttpMethod.Post, request, callback);
        }
    }
}