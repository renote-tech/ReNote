using System.Dynamic;
using Newtonsoft.Json;

namespace Server.Web.Utilities
{
    internal class StreamUtil
    {
        public static async Task<ExpandoObject> ConvertToDynamic(Stream stream)
        {
            using StreamReader sr = new StreamReader(stream);
            return JsonConvert.DeserializeObject<ExpandoObject>(await sr.ReadToEndAsync());
        }
    }
}