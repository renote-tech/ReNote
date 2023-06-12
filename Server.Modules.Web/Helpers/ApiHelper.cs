using System.Collections.Specialized;
using System.Threading.Tasks;
using Server.Common.Encryption;
using Server.ReNote.Management;
using Server.Web.Api;
using Server.Web.Api.Responses;
using Newtonsoft.Json;

namespace Server.Web.Helpers
{
    public class ApiHelper
    {
        /// <summary>
        /// Returns an <see cref="ApiResponse"/>.
        /// </summary>
        /// <param name="status">The status of the <see cref="ApiResponse"/>.</param>
        /// <param name="message">The message of the <see cref="ApiResponse"/>.</param>
        /// <param name="httpStatus">The HTTP status of the <see cref="ApiResponse"/>.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public static async Task<ApiResponse> SendAsync(int status, string message, int httpStatus = default)
        {
            return await Task.Run(() =>
            {
                Response response = new Response() 
                { 
                    Status  = status,
                    Message = message
                };

                int httpResponseStatus = httpStatus == default ? status : httpStatus;
                return new ApiResponse(httpResponseStatus, "application/json", JsonConvert.SerializeObject(response));
            });
        }

        /// <summary>
        /// Returns an <see cref="ApiResponse"/>.
        /// </summary>
        /// <param name="status">The status of the <see cref="ApiResponse"/>.</param>
        /// <param name="message">The mesage of the <see cref="ApiResponse"/>.</param>
        /// <param name="data">The data of the <see cref="ApiResponse"/>.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public static async Task<ApiResponse> SendAsync(int status, string message, object data)
        {
            return await Task.Run(() =>
            {
                DataResponse response = new DataResponse()
                {
                    Status  = status,
                    Message = message,
                    Data    = data
                };

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
            return await SendAsync(500, message);
        }

        /// <summary>
        /// Returns an <see cref="VerificationResponse"/> to check whether the authorization credentials are correct or not.
        /// </summary>
        /// <param name="headers">The headers of an <see cref="ApiResponse"/>.</param>
        /// <returns><see cref="VerificationResponse"/></returns>
        public static async Task<VerificationResponse> VerifyAuthorizationAsync(NameValueCollection headers)
        {
            string sessionId = GetHeaderValue(headers["sessionId"]);
            string authToken = GetHeaderValue(headers["authToken"]);

            VerificationResponse verificationData = new VerificationResponse();

            if (string.IsNullOrWhiteSpace(sessionId) || string.IsNullOrWhiteSpace(authToken))
            {
                verificationData.Response = await SendAsync(400, ApiMessages.EmptySessionIdOrAuthToken());
                return verificationData;
            }

            bool validId = long.TryParse(sessionId, out long realSessionId);
            if (!validId)
            {
                verificationData.Response = await SendAsync(400, ApiMessages.InvalidSession());
                return verificationData;
            }

            Session session = SessionManager.GetSession(realSessionId);

            if (session == null)
            {
                verificationData.Response = await SendAsync(401, ApiMessages.InvalidSession());
                return verificationData;
            }

            if (session.HasExpired())
            {
                verificationData.Response = await SendAsync(401, ApiMessages.InvalidSession());
                return verificationData;
            }

            string computedHash = await Sha256.ComputeStringAsync(authToken);
            if (computedHash != session.AuthToken)
            {
                verificationData.Response = await SendAsync(401, ApiMessages.InvalidSession());
                return verificationData;
            }

            SessionManager.UpdateTimestamp(session.SessionId);

            verificationData.UserId = session.UserId;
            verificationData.SessionId = realSessionId;
            verificationData.Response = await SendAsync(200, ApiMessages.Success());

            return verificationData;
        }

        /// <summary>
        /// Returns the value of an header field.
        /// </summary>
        /// <param name="headerValue">The header to be proceeded.</param>
        /// <returns><see cref="string"/></returns>
        public static string GetHeaderValue(string headerValue)
        {
            if (string.IsNullOrWhiteSpace(headerValue))
                return string.Empty;

            return headerValue.Split(',')[0];
        }
    }
}