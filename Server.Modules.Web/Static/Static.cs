using System.Text;
using Server.Common;
using Server.Common.Utilities;

namespace Server.Web.Static
{
    internal class WebResources
    {
        /// <summary>
        /// The web root path.
        /// </summary>
        public static readonly string WebRoot = PathUtil.NormalizeToOS($"{AppDomain.CurrentDomain.BaseDirectory}/{Configuration.WebConfig.WebRoot}");
        /// <summary>
        /// The index.html path.
        /// </summary>
        public static readonly string Index   = PathUtil.NormalizeToOS($"{WebRoot}/index.html");

        /// <summary>
        /// The internal error message.
        /// </summary>
        public static readonly byte[] InternalError     = Encoding.UTF8.GetBytes("<!DOCTYPE html><html><head><title>Server side error</title></head><body><h2>Unable to send a valid response</h2><hr><p>If you see this page, the server may not be configured correctly.</p></body></html>");
        public static readonly byte[] NotFoundError     = Encoding.UTF8.GetBytes("<!DOCTYPE html><html><head><title>Not Found (404)</title></head><body><h2>Not Found (404)</h2><hr><p>The resource you tried to access was not found.</p></body></html>");
    }

    public enum FileType
    {
        UNKNOWN = 0,
        TEXT    = 1,
        BINARY  = 2,
        VUE     = 3,
        NDWEB   = 4
    }
}