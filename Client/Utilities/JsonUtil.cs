using System.Dynamic;
using System.Text;
using System.Net.Http;

using Newtonsoft.Json;

namespace Client.Utilities
{
    internal class JsonUtil
    {
        public static StringContent SerializeAsBody(ExpandoObject dynamicObject)
        {
            return new StringContent(JsonConvert.SerializeObject(dynamicObject), Encoding.UTF8, "application/json");
        }

        public static ExpandoObject DeserializeAsDynamic(string body)
        {
            return JsonConvert.DeserializeObject<ExpandoObject>(body);
        }
    }
}
