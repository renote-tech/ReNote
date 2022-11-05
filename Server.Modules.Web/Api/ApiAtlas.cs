namespace Server.Web.Api
{
    public class ApiAtlas
    {
        private static readonly List<ApiEndpoint> endpoints = new List<ApiEndpoint>();

        public static void AddEndpoint(ApiEndpoint endpoint)
        {
            endpoints.Add(endpoint);
        }

        public static void AddEndpoint(string name, string uri)
        {
            endpoints.Add(new ApiEndpoint(name, uri));
        }

        public static void RemoveEndpoint(ApiEndpoint endpoint)
        {
            endpoints.Remove(endpoint);
        }

        public static void RemoveEndpoint(string uri)
        {
            ApiEndpoint endpoint = GetEndpoint(uri);
            if (endpoint != null)
                endpoints.Remove(endpoint);
        }

        public static bool EndpointExists(string uri)
        {
            if (GetEndpoint(uri) != null)
                return true;

            return false;
        }

        public static ApiEndpoint GetEndpoint(string uri)
        {
            for(int i = 0; i < endpoints.Count; i++)
            {
                if (endpoints[i].Uri == uri)
                    return endpoints[i];
            }
            return null;
        }

        public static void CleanInternalArray(bool isClear = false)
        {
            if (isClear)
                endpoints.Clear();
            endpoints.TrimExcess();
        }
    }
}