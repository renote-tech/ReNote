using Newtonsoft.Json;
using Server.Common.Utilities;

namespace Server.Web.Utilities
{
    internal class StreamUtil
    {
        /// <summary>
        /// Converts a <see cref="Stream"/> to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The new type of the <see cref="Stream"/>'s data.</typeparam>
        /// <param name="stream">The stream to be converted.</param>
        /// <returns><typeparamref name="T"/></returns>
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