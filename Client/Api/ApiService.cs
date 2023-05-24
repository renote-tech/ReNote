namespace Client.Api;

using Client.Api.Requests;
using Client.Api.Responses;
using Client.Dialogs;
using Client.Layouts;
using Client.ReNote.Data;
using Client.Windows;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

internal class ApiService
{
    private const string GLOBAL_AUTH = "/global/auth";
    private const string SCHOOL_DATA = "/global/school/info";
    private const string QUOTATION = "/global/quotation";
    private const string CONFIGURATION = "/global/client/config";
    private const string USER_PROFILE = "/user/profile";
    private const string USER_LOGOUT = "/user/session/delete";
    private const string TEAM_PROFILE = "/user/team/profile";
    private const string PREFERENCES = "/user/preferences";

    public delegate void RequestCallback<T>(ResponseStatus requestStatus, T response) where T : Response;

    private static HttpClient s_HttpClient;
    private static bool s_Initialized;

    public static void Initialize()
    {
        if (s_Initialized)
            return;

        s_HttpClient = new HttpClient
        {
            BaseAddress = new Uri(Configuration.EndpointAddress),
            Timeout = TimeSpan.FromMilliseconds(7500)
        };

        s_Initialized = true;
    }

    private static async Task SendRequestAsync<TResponse>(string uri, HttpMethod method, Request body, RequestCallback<TResponse> callback) where TResponse : Response, new()
    {
        using HttpRequestMessage request = new HttpRequestMessage(method, uri);

        if (User.Current != null)
        {
            request.Headers.Add("sessionId", User.Current.SessionId.ToString());
            request.Headers.Add("authToken", User.Current.AuthToken);
        }

        if (body != null)
            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

        HttpStatusCode statusCode = HttpStatusCode.ServiceUnavailable;
        string responseBody = string.Empty;

        try
        {
            using HttpResponseMessage response = await s_HttpClient.SendAsync(request);

            statusCode = response.StatusCode;
            responseBody = await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        when (ex is HttpRequestException || ex is TaskCanceledException) { }

        if (callback == null)
            return;

        ResponseStatus responseStatus = GetStatusFromCode(statusCode);
        TResponse responseData = new TResponse();

        if (!string.IsNullOrWhiteSpace(responseBody))
            responseData = JsonConvert.DeserializeObject<TResponse>(responseBody);

        callback(responseStatus, responseData);
    }

    public static async Task GetSchoolDataAsync(RequestCallback<SchoolResponse> callback = null)
    {
        await SendRequestAsync(SCHOOL_DATA, HttpMethod.Get, null, callback);
    }

    public static async Task AuthenticateAsync(AuthRequest request, RequestCallback<AuthResponse> callback = null)
    {
        await SendRequestAsync(GLOBAL_AUTH, HttpMethod.Post, request, callback);
    }

    public static async Task GetQuotationAsync(RequestCallback<QuotationResponse> callback = null)
    {
        await SendRequestAsync(QUOTATION, HttpMethod.Get, null, callback);
    }

    public static async Task GetProfileAsync(RequestCallback<ProfileResponse> callback = null)
    {
        await SendRequestAsync(USER_PROFILE, HttpMethod.Get, null, callback);
    }

    public static async Task LogoutAsync(RequestCallback<Response> callback = null)
    {
        await SendRequestAsync(USER_LOGOUT, HttpMethod.Delete, null, callback);
    }

    public static async Task GetTeamProfileAsync(RequestCallback<TeamProfileResponse> callback = null)
    {
        await SendRequestAsync(TEAM_PROFILE, HttpMethod.Get, null, callback);
    }

    public static async Task GetConfigurationAsync(RequestCallback<ConfigResponse> callback = null)
    {
        await SendRequestAsync(CONFIGURATION, HttpMethod.Get, null, callback);
    }

    public static async Task GetPreferencesAsync(RequestCallback<PreferenceResponse> callback = null)
    {
        await SendRequestAsync(PREFERENCES, HttpMethod.Get, null, callback);
    }

    public static async Task SetPreferencesAsync(PreferenceRequest request, RequestCallback<Response> callback = null)
    {
        await SendRequestAsync(PREFERENCES, HttpMethod.Post, request, callback);
    }

    private static ResponseStatus GetStatusFromCode(HttpStatusCode statusCode)
    {
        switch (statusCode)
        {
            case HttpStatusCode.OK:
                return ResponseStatus.OK;
            case HttpStatusCode.Unauthorized:
                return ResponseStatus.EXPIRED;
            case HttpStatusCode.BadRequest:
                return ResponseStatus.BAD_REQUEST;
            case HttpStatusCode.ServiceUnavailable:
                MainWindow.Instance.SetMaintenanceMode();
                return ResponseStatus.SERVICE_UNAVAILABLE;
            default:
                return ResponseStatus.UNKNOWN;
        }
    }
}

internal enum ResponseStatus
{
    OK                  = 0,
    EXPIRED             = 1,
    BAD_REQUEST         = 2,
    SERVICE_UNAVAILABLE = 3,
    UNKNOWN             = 4
}