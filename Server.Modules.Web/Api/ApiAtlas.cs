using System.Collections.Generic;

namespace Server.Web.Api
{
    public class ApiAtlas
    {
        /// <summary>
        /// The list of <see cref="ApiEndpoint"/>s.
        /// </summary>
        private static readonly List<ApiEndpoint> endpoints = new List<ApiEndpoint>();

        /// <summary>
        /// Register a specified amount of endpoints.
        /// </summary>
        /// <param name="endpoints">The endpoints to register.</param>
        public static void RegisterEndpoints(params string[] endpoints)
        {
            if (endpoints.Length == 0 || endpoints.Length % 2 != 0)
                return;

            for (int i = 0; i < endpoints.Length; i += 2)
                ApiAtlas.endpoints.Add(new ApiEndpoint(endpoints[i], endpoints[i + 1]));

            ApiAtlas.endpoints.TrimExcess();
        }

        /// <summary>
        /// Returns an <see cref="ApiEndpoint"/>.
        /// </summary>
        /// <param name="uri">The uri of the <see cref="ApiEndpoint"/>.</param>
        /// <returns><see cref="ApiEndpoint"/></returns>
        public static ApiEndpoint GetEndpoint(string uri)
        {
            for(int i = 0; i < endpoints.Count; i++)
            {
                if (endpoints[i].Uri == uri)
                    return endpoints[i];
            }
            return null;
        }

        /// <summary>
        /// Returns the number of <see cref="ApiEndpoint"/>s in <see cref="endpoints"/>.
        /// </summary>
        /// <returns><see cref="int"/></returns>
        public static int GetEndpointsCount()
        {
            return endpoints.Count;
        }
    }
}