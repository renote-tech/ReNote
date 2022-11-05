using System.Dynamic;
using Newtonsoft.Json;
using Server.Web.Api;

namespace Server.Web.Utilities
{
    public class ApiUtil
    {
        public static ApiResponse SendBasic(int status, string message)
        {
            dynamic response = new ExpandoObject();
            response.status = status;
            response.message = message;

            return new ApiResponse(status, "application/json", JsonConvert.SerializeObject(response));
        }

        public static ApiResponse SendError(string message)
        {
            return SendBasic(500, message);
        }
    }
}