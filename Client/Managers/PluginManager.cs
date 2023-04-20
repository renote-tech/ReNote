using System.Collections.Generic;

namespace Client.Managers
{
    internal class PluginManager
    {
        public static Dictionary<string, string> Features = new Dictionary<string, string>();

        public static void Initialize(Dictionary<string, string> features)
        {
            Features = features;
        }
    }
}