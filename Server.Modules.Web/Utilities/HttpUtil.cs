using Server.Web.Interfaces;

namespace Server.Web.Utilities
{
    internal class HttpUtil
    {
        public static void RegisterPrefixes(IHttpListener sInterface, string[] prefixes, ushort port)
        {
            if (sInterface == null)
                return;

            if (sInterface.IsDisposed)
                return;

            if (prefixes is null || prefixes.Length == 0)
                prefixes = new string[] { "http://localhost", "http://127.0.0.1" };

            for (int i = 0; i < prefixes.Length; i++)
            {
                string realPrefix = $"{prefixes[i]}:{port}/";
                sInterface.Listener.Prefixes.Add(realPrefix);
            }
        }
    }
}