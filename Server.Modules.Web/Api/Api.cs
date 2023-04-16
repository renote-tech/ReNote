using System.Collections.Specialized;
using System.IO;
using System.Text;
using Server.Common;

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
        {

        }

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
        /// Returns an endpoint not found message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string EndpointNotFound()
        {
            return "Endpoint not found";
        }

        /// <summary>
        /// Returns an endpoint method not found message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string EndpointMethodNotFound()
        {
            return "Endpoint method not found";
        }

        /// <summary>
        /// Returns a success message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string Success()
        {
            return "Success";
        }

        /// <summary>
        /// Returns a method not allowed message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string MethodNotAllowed()
        {
            return "Method not allowed";
        }

        /// <summary>
        /// Returns an invalid json message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string InvalidJson()
        {
            return "JSON syntax is invalid";
        }

        /// <summary>
        /// Returns a null or empty message.
        /// </summary>
        /// <param name="properties">The list of <see cref="string"/>.</param>
        /// <returns><see cref="string"/></returns>
        public static string NullOrEmpty(params string[] properties)
        {
            if (properties.Length == 0)
                return string.Empty;
            else if (properties.Length == 1)
                return $"{properties[0]} may not be empty";

            StringBuilder baseMessage = new StringBuilder(); 
            for(int i = 0; i < properties.Length; i++)
            {
                if (i > 0 && i == properties.Length - 1)
                    baseMessage.Append(" and ");
                else if (i > 0 && i < properties.Length - 2)
                    baseMessage.Append(", ");

                baseMessage.Append($"{properties[i]}");
            }
            baseMessage.Append(" may not be empty");
            return baseMessage.ToString();
        }

        /// <summary>
        /// Returns an empty username or password message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string EmptyUsernameOrPassword()
        {
            return "LogonEmptyField";
        }

        /// <summary>
        /// Returns an invalid username or password message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string InvalidUsernameOrPassword()
        {
            return "LogonInvalidField";
        }

        /// <summary>
        /// Returns a session not exists or expired message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string InvalidSession()
        {
            return "Session either doesn't exist, is expired or provided token is invalid";
        }
    }

    public class ApiStatus
    {
        public const int SESSION_EXPIRED = 1;
    }

    public class ApiRegisterer
    {
        /// <summary>
        /// Registers all of the <see cref="ApiEndpoint"/>s.
        /// </summary>
        public static void Initialize()
        {
            ApiAtlas.RegisterEndpoints("Root", "/",

                                       "Authenticate",  $"/global/{ServerInfo.ApiVersion}/auth",
                                       "SchoolInfo",    $"/global/{ServerInfo.ApiVersion}/school/info",
                                       "About",         $"/global/{ServerInfo.ApiVersion}/about",
                                       "Quotation",     $"/global/{ServerInfo.ApiVersion}/quotation",
                                       "ColorSchema",   $"/global/{ServerInfo.ApiVersion}/color/themes",
                                       "Configuration", $"/global/{ServerInfo.ApiVersion}/client/config",

                                       "Profile",       $"/user/{ServerInfo.ApiVersion}/profile",
                                       "Preferences",   $"/user/{ServerInfo.ApiVersion}/preferences",
                                       "Timetable",     $"/user/{ServerInfo.ApiVersion}/timetable",
                                       "LogOut",        $"/user/{ServerInfo.ApiVersion}/session/delete",
                                       "TeamProfile",   $"/user/{ServerInfo.ApiVersion}/team/profile");

            Platform.Log($"Registered {ApiAtlas.GetEndpointsCount()} endpoints", LogLevel.INFO);
        }
    }
}