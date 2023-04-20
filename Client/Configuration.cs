using System;
using System.IO;
using Newtonsoft.Json;

namespace Client
{
    internal class Configuration
    {
        public static string EndpointAddress { get; private set; }
        public static string Language { get; private set; }

        private const string DEFAULT_ADDRESS  = "http://127.0.0.1:7101";
        private const string DEFAULT_LANGUAGE = "en-GB";

        private class ClientConfig
        {
            [JsonProperty("endpointAddr")]
            public string EndpointAddress { get; set; }

            [JsonProperty("language")]
            public string Language { get; set; }
        }

        public static void LoadAll()
        {
            Load("Global");
            Load("Local", true);

            if (EndpointAddress == null)
                EndpointAddress = DEFAULT_ADDRESS;

            if (Language == null)
                Language = DEFAULT_LANGUAGE;
        }

        private static void Load(string name, bool localConfig = false)
        {
            string configFileName = $"{name.ToLower()}.config.json";
            string configFilePath = configFileName;
            if (localConfig)
                configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                                              "ReNote", configFilePath);

            if (!File.Exists(configFilePath))
                return;

            string content = File.ReadAllText(configFilePath);
            if (string.IsNullOrWhiteSpace(content))
                return;

            try
            {
                ClientConfig config = JsonConvert.DeserializeObject<ClientConfig>(content);
                EndpointAddress = config.EndpointAddress;
                Language = config.Language;

                Platform.Log($"Loaded {name} configuration", LogLevel.INFO);
            } catch { }
        }

        public static void Save(string name, string langCode)
        {
            string content = JsonConvert.SerializeObject(new ClientConfig()
            {
                EndpointAddress = EndpointAddress,
                Language = langCode ?? Language
            });

            string configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                "ReNote", $"{name.ToLower()}.config.json");

            File.WriteAllTextAsync(configFilePath, content);
        }
    }
}