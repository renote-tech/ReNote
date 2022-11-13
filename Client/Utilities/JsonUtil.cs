using System.Dynamic;
using System.Text;
using Newtonsoft.Json;

namespace Client.Utilities
{
    internal class JsonUtil
    {
        public static StringContent SerializeAsBody(ExpandoObject dynamicObject)
        {
            return new StringContent(JsonConvert.SerializeObject(dynamicObject), Encoding.UTF8, "application/json");
        }
    }
}