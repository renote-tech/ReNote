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
        /// <summary>
        /// Returns an <see cref="ApiResponse"/>.
        /// </summary>
        /// <param name="status">The status of the <see cref="ApiResponse"/>.</param>
        /// <param name="message">The message of the <see cref="ApiResponse"/>.</param>
        /// <returns><see cref="ApiResponse"/></returns>
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

        /// <summary>
        /// Returns an <see cref="ApiResponse"/>.
        /// </summary>
        /// <param name="httpStatus">The HTTP status of the <see cref="ApiResponse"/>.</param>
        /// <param name="status">The status of the <see cref="ApiResponse"/>.</param>
        /// <param name="message">The message of the <see cref="ApiResponse"/>.</param>
        /// <returns><see cref="ApiResponse"/></returns>
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

        /// <summary>
        /// Returns an <see cref="ApiResponse"/>.
        /// </summary>
        /// <param name="status">The status of the <see cref="ApiResponse"/>.</param>
        /// <param name="message">The mesage of the <see cref="ApiResponse"/>.</param>
        /// <param name="data">The data of the <see cref="ApiResponse"/>.</param>
        /// <returns><see cref="ApiResponse"/></returns>
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

        /// <summary>
        /// Returns an <see cref="ApiResponse"/>.
        /// </summary>
        /// <param name="message">The error message of the <see cref="ApiResponse"/>.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public static async Task<ApiResponse> SendErrorAsync(string message)
        {
            return await Task.Run(() => SendAsync(500, message));
        }

        /// <summary>
        /// Returns an <see cref="ApiResponse"/> to check whether the authorization credentials are correct or not.
        /// </summary>
        /// <param name="headers">The headers of an <see cref="ApiResponse"/>.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public static async Task<ApiResponse> VerifyAuthorizationAsync(NameValueCollection headers)
        {
            string sessionId = GetHeaderFirstValue(headers["sessionId"]);
            string authToken = GetHeaderFirstValue(headers["authToken"]);

            if (string.IsNullOrWhiteSpace(sessionId) || string.IsNullOrWhiteSpace(authToken))
                return await SendAsync(400, ApiMessages.NullOrEmpty("userId", "authToken"));

            if (!NumberUtil.IsSafeLong(sessionId))
                return await SendAsync(400, ApiMessages.InvalidSessionId());

            long realSessionId = long.Parse(sessionId);

            GlobalSession session = SessionManager.GetSession(realSessionId);

            if (session == null)
                return await SendAsync(400, ApiMessages.SessionNotExists());

            if (session.HasExpired())
                return await SendAsync(401, (int)ApiStatus.SESSION_EXPIRED, ApiMessages.SessionExpired());

            if (EncryptionUtil.ComputeStringSha256(authToken) != session.AuthToken)
                return await SendAsync(401, ApiMessages.InvalidAuthToken());

            SessionManager.UpdateSessionTimestamp(session.SessionId);

            return await SendWithDataAsync(200, ApiMessages.Success(), session.UserId);
        }

        /// <summary>
        /// Returns the first value of an header value field.
        /// </summary>
        /// <param name="headerValue">The header to be proceeded.</param>
        /// <returns><see cref="string"/></returns>
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