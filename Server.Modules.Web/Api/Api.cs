using System.Collections.Specialized;
using System.Data;
using System.Text;

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
        public static string MethodNotAllowed()
        {
            return "Method not allowed";
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
    }

    public class ApiRegisterer
    {
        public static void Initialize()
        {
            ApiAtlas.AddEndpoint("Authenticate", "/global/v1/auth");
        }
    }
}