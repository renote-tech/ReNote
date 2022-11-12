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
        /// <param name="endpoint">The <see cref="ApiEndpoint"/> to be added.</param>
        public static void AddEndpoint(ApiEndpoint endpoint)
        {
            endpoints.Add(endpoint);
            Clean();
        }

        /// <summary>
        /// Adds an <see cref="ApiEndpoint"/> to the <see cref="endpoints"/> list.
        /// </summary>
        /// <param name="name">The name of the <see cref="ApiEndpoint"/>.</param>
        /// <param name="uri">The uri of the <see cref="ApiEndpoint"/>.</param>
        public static void AddEndpoint(string name, string uri)
        {
            endpoints.Add(new ApiEndpoint(name, uri));
            Clean();
        }

        /// <summary>
        /// Removes an <see cref="ApiEndpoint"/> from the <see cref="endpoints"/> list.
        /// </summary>
        /// <param name="endpoint">The <see cref="ApiEndpoint"/> to be removed.</param>
        public static void RemoveEndpoint(ApiEndpoint endpoint)
        {
            bool result = endpoints.Remove(endpoint);
            if(result)
                Clean();
        }

        /// <summary>
        /// Removes an <see cref="ApiEndpoint"/> from the <see cref="endpoints"/> list.
        /// </summary>
        /// <param name="uri">The uri of the <see cref="ApiEndpoint"/>.</param>
        public static void RemoveEndpoint(string uri)
        {
            bool result = false;

            ApiEndpoint endpoint = GetEndpoint(uri);
            if (endpoint != null)
                result = endpoints.Remove(endpoint);
            
            if(result)
                Clean();
        }

        /// <summary>
        /// Returns whether the <see cref="ApiEndpoint"/> exists.
        /// </summary>
        /// <param name="uri">The uri of the <see cref="ApiEndpoint"/>.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool EndpointExists(string uri)
        {
            if (GetEndpoint(uri) != null)
                return true;

            return false;
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
        /// Clears the <see cref="endpoints"/> list.
        /// </summary>
        public static void Clear()
        {
            endpoints.Clear();
        }

        /// <summary>
        /// Cleans up the <see cref="endpoints"/>'s internal array.
        /// </summary>
        private static void Clean()
        {
            endpoints.TrimExcess();
        }
    }
}