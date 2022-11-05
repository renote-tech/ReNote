using Server.Common.Utilities;

namespace Server.Web.Static
{
    internal class StaticData
    {
        public static async Task<byte[]> GetResource(string uri)
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

        public static async Task<bool> ResourceExists(string uri)
        {
            return await GetResource(uri) == WebResources.InternalError;
        }
    }
}