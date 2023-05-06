using System;
using System.IO;
using System.Text;
using Server.Common;

namespace Server.Web.Static
{
    internal class WebResources
    {
        /// <summary>
        /// The web root path.
        /// </summary>
        public static readonly string WebRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Configuration.WebConfig.WebRoot);
        /// <summary>
        /// The index.html path.
        /// </summary>
        public static readonly string Index = Path.Combine(WebRoot, "index.html");
        /// <summary>
        /// The not found error message.
        /// </summary>
        public static readonly byte[] NotFoundError = Encoding.UTF8.GetBytes("<!DOCTYPE html><html><head><title>Not Found (404)</title></head><body><h2>Not Found (404)</h2><hr><p>The resource you tried to access was not found.</p></body></html>");
    }
}