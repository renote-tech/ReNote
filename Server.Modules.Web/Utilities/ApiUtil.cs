﻿using System.Collections.Specialized;
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
        /// <param name="httpStatus">The HTTP status of the <see cref="ApiResponse"/>.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public static async Task<ApiResponse> SendAsync(int status, string message, int httpStatus = default)
        {
            return await Task.Run(() =>
            {
                BasicResponse response = new BasicResponse() 
                { 
                    Status  = status,
                    Message = message
                };

                return new ApiResponse((httpStatus == default ? status : httpStatus), "application/json", JsonConvert.SerializeObject(response));
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
        /// <returns><see cref="ApiResponse"/></returns>
        public static ApiResponse SendNoData(int httpStatus)
        {
            return new ApiResponse() { Status = httpStatus };
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
        /// Returns an <see cref="ApiResponse"/> to check whether the authorization credentials are correct or not.
        /// </summary>
        /// <param name="headers">The headers of an <see cref="ApiResponse"/>.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public static async Task<ApiResponse> VerifyAuthorizationAsync(NameValueCollection headers)
        {
            string sessionId = GetHeaderValue(headers["sessionId"]);
            string authToken = GetHeaderValue(headers["authToken"]);

            if (string.IsNullOrWhiteSpace(sessionId) || string.IsNullOrWhiteSpace(authToken))
                return await SendAsync(400, ApiMessages.NullOrEmpty("sessionId", "authToken"));

            if (!NumberUtil.IsSafeLong(sessionId))
                return await SendAsync(400, ApiMessages.InvalidSessionId());

            long realSessionId = long.Parse(sessionId);

            GlobalSession session = SessionManager.GetSession(realSessionId);

            if (session == null)
                return await SendAsync(400, ApiMessages.SessionNotExists());

            if (session.HasExpired())
                return await SendAsync(ApiStatus.SESSION_EXPIRED, ApiMessages.SessionExpired(), 401);

            string computedHash = await EncryptionUtil.ComputeStringSha256Async(authToken);
            if (computedHash != session.AuthToken)
                return await SendAsync(401, ApiMessages.InvalidAuthToken());

            SessionManager.UpdateRequestTimestamp(session.SessionId);

            return await SendWithDataAsync(200, ApiMessages.Success(), session.UserId);
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

            if (!headerValue.Contains(','))
                return headerValue;

            return headerValue.Split(',')[0];
        }
    }
}