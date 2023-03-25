namespace Server.Web.Api
{
    public class ApiAtlas
    {
        /// <summary>
        /// The list of <see cref="ApiEndpoint"/>s.
        /// </summary>
        private static readonly List<ApiEndpoint> endpoints = new List<ApiEndpoint>();

        /// <summary>
        /// Adds an <see cref="ApiEndpoint"/> to the <see cref="endpoints"/> list.
        /// </summary>
        /// <param name="name">The name of the <see cref="ApiEndpoint"/>.</param>
        /// <param name="uri">The uri of the <see cref="ApiEndpoint"/>.</param>
        public static void AddEndpoint(string name, string uri)
        {
            endpoints.Add(new ApiEndpoint($"{name}", uri));
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

        /// <summary>
        /// Cleans up the <see cref="endpoints"/>'s internal array.
        /// </summary>
        public static void Clean()
        {
            endpoints.TrimExcess();
        }
    }
}