using Server.Common;
using Server.Web.Interfaces;

namespace Server.Web.Utilities
{
    internal class HttpUtil
    {
        /// <summary>
        /// Registers the prefixes of an interface.
        /// </summary>
        /// <param name="sInterface">The <see cref="IHttpListener"/> to be proceeded.</param>
        /// <param name="prefixes">The list of prefixes.</param>
        /// <param name="port">The <see cref="IHttpListener"/> port.</param>
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

            Platform.Log($"Initialized IHttpListener; PORT={port}", LogLevel.INFO);
        }
    }
}