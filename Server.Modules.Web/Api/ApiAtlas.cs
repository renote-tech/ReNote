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
        /// Register a specified amount of endpoints.
        /// </summary>
        /// <param name="endpoints">The endpoints to register.</param>
        public static void RegisterEndpoints(params string[] endpoints)
        {
            if (endpoints.Length == 0 || endpoints.Length % 2 != 0)
                return;

            for (int i = 0; i < endpoints.Length; i += 2)
                s_Endpoints.Add(new ApiEndpoint(endpoints[i], endpoints[i + 1]));

            s_Endpoints.TrimExcess();
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