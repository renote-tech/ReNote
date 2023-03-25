using System.Collections.Specialized;
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
            StringBuilder baseMessage = new StringBuilder(); 
            for(int i = 0; i < properties.Length; i++)
            {
                baseMessage.Append($"{properties[i]}");
                if(i != properties.Length - 1)
                    baseMessage.Append(", ");
            }
            baseMessage.Append(" may not be empty");
            return baseMessage.ToString();
        }

        /// <summary>
        /// Returns an user not exists message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string UserNotExists()
        {
            return "User doesn't exist";
        }

        /// <summary>
        /// Returns an invalid password message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string InvalidPassword()
        {
            return "Password is invalid";
        }

        /// <summary>
        /// Returns a session not exists message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string SessionNotExists()
        {
            return "Session doesn't exist";
        }

        /// <summary>
        /// Returns a session expired message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string SessionExpired()
        {
            return "Session has expired";
        }

        /// <summary>
        /// Returns an invalid auth token message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string InvalidAuthToken()
        {
            return "Auth token is invalid";
        }

        /// <summary>
        /// Returns an invalid session id message.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        public static string InvalidSessionId()
        {
            return "Session ID is invalid";
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
            ApiAtlas.AddEndpoint("Root", "/");

            ApiAtlas.AddEndpoint("Authenticate", $"/global/{ServerEnv.ApiVersion}/auth");
            ApiAtlas.AddEndpoint("SchoolInfo",   $"/global/{ServerEnv.ApiVersion}/school/info");
            ApiAtlas.AddEndpoint("About",        $"/global/{ServerEnv.ApiVersion}/about");
            ApiAtlas.AddEndpoint("Quotation",    $"/global/{ServerEnv.ApiVersion}/quotation");

            ApiAtlas.AddEndpoint("Profile",      $"/user/{ServerEnv.ApiVersion}/profile");
            ApiAtlas.AddEndpoint("Preferences",  $"/user/{ServerEnv.ApiVersion}/preferences");
            ApiAtlas.AddEndpoint("Timetable",    $"/user/{ServerEnv.ApiVersion}/timetable");
            ApiAtlas.AddEndpoint("LogOut",       $"/user/{ServerEnv.ApiVersion}/session/delete");

            ApiAtlas.Clean();
            Platform.Log($"Registered {ApiAtlas.GetEndpointsCount()} endpoints", LogLevel.INFO);
        }
    }
}