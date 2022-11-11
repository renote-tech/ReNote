using Newtonsoft.Json;
using Server.Common.Utilities;

namespace Server.Web.Utilities
{
    internal class StreamUtil
    {
        public static async Task<T> Convert<T>(Stream stream) where T : class
        {
            using StreamReader sr = new StreamReader(stream);
            string content = await sr.ReadToEndAsync();
            if (!JsonUtil.ValiditateJson(content))
                return null;
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}