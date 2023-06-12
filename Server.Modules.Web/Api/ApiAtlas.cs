using Server.Common;
using System.Collections.Generic;

namespace Server.Web.Api
{
    public class ApiAtlas
    {
        /// <summary>
        /// The list of <see cref="ApiEndpoint"/>s.
        /// </summary>
        private static readonly List<ApiEndpoint> s_Endpoints = new List<ApiEndpoint>();

        /// <summary>
        /// True if the <see cref="Initialize(string[])"/> method has been called, otherwise false.
        /// </summary>
        private static bool s_Initialized;

        /// <summary>
        /// Register the specified endpoints.
        /// </summary>
        /// <param name="endpoints">The endpoints to register.</param>
        public static void Initialize(params string[] endpoints)
        {
            if (s_Initialized)
                return;

            if (endpoints.Length == 0 || endpoints.Length % 2 != 0)
                return;

            for (int i = 0; i < endpoints.Length; i += 2)
                s_Endpoints.Add(new ApiEndpoint(endpoints[i], endpoints[i + 1]));

            Platform.Log($"Registered {GetEndpointsCount()} endpoints", LogLevel.INFO);

            s_Endpoints.TrimExcess();

            s_Initialized = true;
        }

        /// <summary>
        /// Returns an <see cref="ApiEndpoint"/>.
        /// </summary>
        /// <param name="uri">The uri of the <see cref="ApiEndpoint"/>.</param>
        /// <returns><see cref="ApiEndpoint"/></returns>
        public static ApiEndpoint GetEndpoint(string uri)
        {
            for(int i = 0; i < s_Endpoints.Count; i++)
            {
                if (s_Endpoints[i].Uri == uri)
                    return s_Endpoints[i];
            }
            return null;
        }

        /// <summary>
        /// Returns the number of <see cref="ApiEndpoint"/>s in <see cref="s_Endpoints"/>.
        /// </summary>
        /// <returns><see cref="int"/></returns>
        public static int GetEndpointsCount()
        {
            return s_Endpoints.Count;
        }
    }
}