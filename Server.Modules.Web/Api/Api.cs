using System.Collections.Specialized;
using System.Text;
using Server.Common.Utilities;

namespace Server.Web.Api
{
    public class ApiEndpoint
    {
        public string Name { get; set; }
        public string Uri { get; set; }

        public ApiEndpoint(string name, string uri)
        {
            Name = name;
            Uri = uri;
        }
    }

    public class ApiRequest
    {
        public string Method { get; set; }
        public NameValueCollection Headers { get; set; }
        public NameValueCollection Query { get; set; }
        public Stream Body { get; set; }
    }

    public class ApiResponse
    {
        public int Status { get; set; }
        public string ContentType { get; set; }
        public string Body { get; set; }

        public ApiResponse(int status, string contentType, string body)
        {
            Status = status;
            ContentType = contentType;
            Body = body;
        }
    }

    public class ApiMessages
    {
        public static string Success()
        {
            return "Success";
        }

        public static string MethodNotAllowed()
        {
            return "Method not allowed";
        }

        public static string InvalidJson()
        {
            return "JSON syntax is invalid";
        }

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

        public static string UserNotExists()
        {
            return "User doesn't exist";
        }

        public static string InvalidPassword()
        {
            return "Password is invalid";
        }

        public static string SessionNotExists()
        {
            return "Session doesn't exist";
        }

        public static string SessionExpired()
        {
            return "Session has expired";
        }

        public static string InvalidAuthToken()
        {
            return "Auth token is invalid";
        }
    }

    public enum ApiStatus
    {
        SESSION_EXPIRED = 1
    }

    public class ApiRegisterer
    {
        public static void Initialize()
        {
            ApiAtlas.AddEndpoint("Root", "/");
            ApiAtlas.AddEndpoint("Authenticate", $"/global/{ServerEnvironment.ServerApiVersion}/auth");
            ApiAtlas.AddEndpoint("SchoolInfo", $"/global/{ServerEnvironment.ServerApiVersion}/school/info");
            ApiAtlas.AddEndpoint("Profile", $"/user/{ServerEnvironment.ServerApiVersion}/profile");
        }
    }
}