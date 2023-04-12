using System.IO;
using Newtonsoft.Json;

namespace Client
{
    internal class Configuration
    {
        public static string EndpointAddress { get; private set; }
        public static string Language { get; private set; }

        private static string s_DefaultAddress  = "http://127.0.0.1:7101";
        private static string s_DefaultLanguage = "en_GB";

        private class ClientConfig
        {
            [JsonProperty("endpointAddr")]
            public string EndpointAddress { get; set; }

            [JsonProperty("language")]
            public string Language { get; set; }
        }

        public static void Load()
        {
            string configFileName = "global.config.json";

            if (!File.Exists(configFileName))
            {
                EndpointAddress = s_DefaultAddress;
                Language = s_DefaultLanguage;
                return;
            }

            string configContent = File.ReadAllText(configFileName);
            if (string.IsNullOrWhiteSpace(configContent))
            {
                EndpointAddress = s_DefaultAddress;
                Language = s_DefaultLanguage;
                return;
            }

            try
            {
                ClientConfig config = JsonConvert.DeserializeObject<ClientConfig>(configContent);
                EndpointAddress = config.EndpointAddress ?? s_DefaultAddress;
                Language = config.Language ?? s_DefaultLanguage;

                Platform.Log($"Loaded Client configuration", LogLevel.INFO);
            } catch { }
        }
    }
}