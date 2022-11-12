using Server.Common.Utilities;

namespace Server.Web.Static
{
    internal class StaticData
    {
        /// <summary>
        /// Returns a resource from the specified uri.
        /// </summary>
        /// <param name="uri">The resource uri.</param>
        /// <returns><see cref="byte"/>[]</returns>
        public static async Task<byte[]> GetResourceAsync(string uri)
        {
            string resourcePath;
            if (uri == "/" || string.IsNullOrWhiteSpace(uri))
                resourcePath = WebResources.Index;
            else if (Configuration.WebConfig.WebNoDotHtml)
                resourcePath = PathUtil.NormalizeToOS($"{WebResources.WebRoot}/{uri}/index.html");
            else if (!Configuration.WebConfig.WebVueSupport)
                resourcePath = PathUtil.NormalizeToOS($"{WebResources.WebRoot}/{uri}");
            else
                resourcePath = WebResources.Index;

            if (!File.Exists(resourcePath))
                return WebResources.InternalError;

            return await File.ReadAllBytesAsync(resourcePath);
        }

        /// <summary>
        /// Returns whether the resource exists or not.
        /// </summary>
        /// <param name="uri">The resource uri.</param>
        /// <returns><see cref="bool"/></returns>
        public static async Task<bool> ResourceExistsAsync(string uri)
        {
            return await GetResourceAsync(uri) == WebResources.InternalError;
        }
    }
}