using System;
using System.IO;
using Client.Logging;
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

            [JsonProperty("super_duper_not_secret_backup_endpoint_address")]
            public string BackupEndpointAddress { get; set; }

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
                if (!localConfig)
                    EndpointAddress = config.EndpointAddress;
                else if (localConfig && config.BackupEndpointAddress != null)
                    EndpointAddress = config.BackupEndpointAddress;

                Language = config.Language;

                Platform.Log($"Loaded {name} configuration", LogLevel.INFO);
            } catch { }
        }

        public static void Save(string name, string langCode)
        {
            string content = JsonConvert.SerializeObject(new ClientConfig() { Language = langCode ?? Language },
                                                         new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

            string renoteDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ReNote");
            if (!Directory.Exists(renoteDirectory))
                Directory.CreateDirectory(renoteDirectory);

            string configFilePath = Path.Combine(renoteDirectory, $"{name.ToLower()}.config.json");
            File.WriteAllTextAsync(configFilePath, content);
        }
    }
}