using System.Collections.Specialized;
using Newtonsoft.Json;
using Server.Common.Utilities;
using Server.ReNote.Management;
using Server.Web.Api;
using Server.Web.Api.Responses;

namespace Server.Web.Utilities
{
    public class ApiUtil
    {
        public static async Task<ApiResponse> SendAsync(int status, string message)
        {
            return await Task.Run(() =>
            {
                BasicResponse response = new BasicResponse();
                response.Status = status;
                response.Message = message;

                return new ApiResponse(status, "application/json", JsonConvert.SerializeObject(response));
            });
        }

        public static async Task<ApiResponse> SendAsync(int httpStatus, int status, string message)
        {
            return await Task.Run(() =>
            {
                BasicResponse response = new BasicResponse();
                response.Status = status;
                response.Message = message;

                return new ApiResponse(httpStatus, "application/json", JsonConvert.SerializeObject(response));
            });
        }

        public static async Task<ApiResponse> SendWithDataAsync(int status, string message, object data)
        {
            return await Task.Run(() =>
            {
                DataResponse response = new DataResponse();
                response.Status = status;
                response.Message = message;
                response.Data = data;

                return new ApiResponse(status, "application/json", JsonConvert.SerializeObject(response));
            });
        }

        public static async Task<ApiResponse> SendErrorAsync(string message)
        {
            return await Task.Run(() => SendAsync(500, message));
        }

        public static async Task<ApiResponse> VerifyAuthorization(NameValueCollection headers)
        {
            string sessionId = GetHeaderFirstValue(headers["sessionId"]);
            string authToken = GetHeaderFirstValue(headers["authToken"]);

            if (string.IsNullOrWhiteSpace(sessionId) || string.IsNullOrWhiteSpace(authToken))
                return await SendAsync(400, ApiMessages.NullOrEmpty("userId", "authToken"));

            GlobalSession session = SessionManager.GetSession(sessionId);

            if (session == null)
                return await SendAsync(400, ApiMessages.SessionNotExists());

            if (session.HasExpired())
                return await SendAsync(401, (int)ApiStatus.SESSION_EXPIRED, ApiMessages.SessionExpired());

            if (EncryptionUtil.ComputeStringSha256(authToken) != session.AuthToken)
                return await SendAsync(401, ApiMessages.InvalidAuthToken());

            SessionManager.UpdateSessionTimestamp(sessionId);

            return await SendAsync(200, ApiMessages.Success());
        }

        private static string GetHeaderFirstValue(string headerValue)
        {
            if (string.IsNullOrWhiteSpace(headerValue))
                return string.Empty;

            if (!headerValue.Contains(','))
                return headerValue;

            return headerValue.Split(',')[0];
        }
    }
}