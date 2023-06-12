using System.Collections.Specialized;
using System.IO;

namespace Server.Web.Api
{
    public class ApiEndpoint
    {
        /// <summary>
        /// The API name of the <see cref="ApiEndpoint"/>.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The API uri of the <see cref="ApiEndpoint"/>.
        /// </summary>
        public string Uri { get; set; }

        public ApiEndpoint(string name, string uri)
        {
            Name = name;
            Uri = uri;
        }
    }

    public class ApiRequest
    {
        /// <summary>
        /// The API method of the <see cref="ApiRequest"/>.
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// The headers of the <see cref="ApiRequest"/>.
        /// </summary>
        public NameValueCollection Headers { get; set; }
        /// <summary>
        /// The query parameters of the <see cref="ApiRequest"/>.
        /// </summary>
        public NameValueCollection Query { get; set; }
        /// <summary>
        /// The body of the <see cref="ApiRequest"/>.
        /// </summary>
        public Stream Body { get; set; }
    }

    public class ApiResponse
    {
        public static readonly ApiResponse OK = new ApiResponse(200, string.Empty, string.Empty);
        
        /// <summary>
        /// The status of the <see cref="ApiResponse"/>.
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// The content type of the <see cref="ApiResponse"/>.
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// The body of the <see cref="ApiResponse"/>.
        /// </summary>
        public string Body { get; set; }

        public ApiResponse()
        { }

        public ApiResponse(int status, string contentType, string body)
        {
            Status = status;
            ContentType = contentType;
            Body = body;
        }
    }

    public class ApiMessages
    {
        /// <summary>
        /// Returns an "endpoint not found" message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string EndpointNotFound()
        {
            return "Endpoint not found";
        }

        /// <summary>
        /// Returns an "endpoint method not found" message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string EndpointMethodNotFound()
        {
            return "Endpoint method not found";
        }

        /// <summary>
        /// Returns a "success" message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string Success()
        {
            return "Success";
        }

        /// <summary>
        /// Returns a "method not allowed" message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string MethodNotAllowed()
        {
            return "Method not allowed";
        }

        /// <summary>
        /// Returns an "invalid json" message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string InvalidJson()
        {
            return "JSON syntax is invalid";
        }

        /// <summary>
        /// Returns an "empty sessionId or authToken" message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string EmptySessionIdOrAuthToken()
        {
            return "Session ID and auth token may not be empty";
        }

        /// <summary>
        /// Returns an "empty username or password" message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string EmptyUsernameOrPassword()
        {
            return "LogonEmptyField";
        }

        /// <summary>
        /// Returns an "empty current password or new password" message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string EmptyCurrentPasswordOrNewPassword()
        {
            return "DialogPasswordEmptyField";
        }

        /// <summary>
        /// Returns an "cannot use same password" message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string CannotUseSamePassword()
        {
            return "DialogCannotUseSamePassword";
        }

        /// <summary>
        /// Returns an "invalid password" message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string InvalidPassword()
        {
            return "DialogInvalidPassword";
        }

        /// <summary>
        /// Returns an "invalid username or password" message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string InvalidUsernameOrPassword()
        {
            return "LogonInvalidField";
        }

        /// <summary>
        /// Returns a "session not exists or expired" message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string InvalidSession()
        {
            return "Session either doesn't exist, is expired or provided token is invalid";
        }

        /// <summary>
        /// Returns a "server is running" message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string ServerRunning()
        {
            return "Server is running";
        }

        /// <summary>
        /// Returns a "empty language or theme" message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string EmptyLanguageOrTheme()
        {
            return "Language or theme may not be empty";
        }

        /// <summary>
        /// Returns a "plugin disabled" message.
        /// </summary>
        /// <returns></returns>
        public static string PluginDisabled()
        {
            return "This plugin has been disabled by your administrator";
        }
    }
}