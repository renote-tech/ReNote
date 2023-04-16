using System;
using System.IO;
using Newtonsoft.Json;
using Server.Common.Utilities;

namespace Server.Common
{
    public class Configuration
    {
        /// <summary>
        /// True if the <see cref="LoadAllConfigurations"/> method has been called; otherwise false.
        /// </summary>
        public static bool IsConfigurationsLoaded { get; private set; }
        /// <summary>
        /// The <see cref="GlobalConfig"/> object.
        /// </summary>
        public static GlobalConfig GlobalConfig { get; set; }
        /// <summary>
        /// The <see cref="WebConfig"/> object.
        /// </summary>
        public static WebConfig WebConfig { get; set; }
        /// <summary>
        /// The <see cref="ReNoteConfig"/> object.
        /// </summary>
        public static ReNoteConfig ReNoteConfig { get; set; }

        /// <summary>
        /// Loads all of the specified configurations.
        /// </summary>
        public static void LoadAllConfigurations()
        {
            if (IsConfigurationsLoaded)
                return;

            GlobalConfig = LoadConfiguration<GlobalConfig>("Global");
            WebConfig    = LoadConfiguration<WebConfig>("Web");
            ReNoteConfig = LoadConfiguration<ReNoteConfig>("ReNote");

            IsConfigurationsLoaded = true;
        }

        /// <summary>
        /// Loads a specific configuration.
        /// </summary>
        /// <typeparam name="T">The configuration type.</typeparam>
        /// <param name="name">The configuration name.</param>
        /// <returns><typeparamref name="T"/></returns>
        public static T LoadConfiguration<T>(string name) where T : new()
        {
            T config;
            string configFileName = $"{name.ToLower()}.config.json";

            if (!File.Exists(configFileName))
                return new T();

            string configContent = File.ReadAllText(configFileName);

            if (string.IsNullOrWhiteSpace(configContent))
                return new T();

            if (JsonUtil.ValiditateJson(configContent))
                config = JsonConvert.DeserializeObject<T>(configContent);
            else
                config = new T();

            Platform.Log($"Loaded {name} configuration", LogLevel.INFO);

            return config;
        }

        /// <summary>
        /// Saves all of the specified configurations.
        /// </summary>
        public static void SaveAllConfigurations()
        {
            SaveConfigurationAsync("Global", GlobalConfig);
            SaveConfigurationAsync("Web", WebConfig);
            SaveConfigurationAsync("ReNote", ReNoteConfig);
        }

        /// <summary>
        /// Saves a specific configuration.
        /// </summary>
        /// <typeparam name="T">The configuration type.</typeparam>
        /// <param name="name">The configuration name.</param>
        /// <param name="configObject">The configuration object.</param>
        public static async void SaveConfigurationAsync<T>(string name, T configObject)
        {
            string data = JsonConvert.SerializeObject(configObject);
            await File.WriteAllTextAsync($"{name.ToLower()}.config.json", data);
        }
    }

    public class GlobalConfig
    {
        /// <summary>
        /// The HTTP urls prefixes.
        /// </summary>
        [JsonProperty("prefixes")]
        public string[] Prefixes { get; set; }
        /// <summary>
        /// The web interface port.
        /// </summary>
        [JsonProperty("webPort")]
        public ushort WebPort { get; set; }
        /// <summary>
        /// The API interface port.
        /// </summary>
        [JsonProperty("apiPort")]
        public ushort ApiPort { get; set; }
        /// <summary>
        /// The socket interface port.
        /// </summary>
        [JsonProperty("socketPort")]
        public ushort SocketPort { get; set; }

        public GlobalConfig() 
        {
            Prefixes = Array.Empty<string>(); 
        }
    }

    public class WebConfig
    {
        /// <summary>
        /// The relative root path of the web interface.
        /// </summary>
        [JsonProperty("webRoot")]
        public string WebRoot { get; set; }

        /// <summary>
        /// The web interface parameter to modify the behavior of the urls.
        /// </summary>
        [JsonProperty("webNoDotHtml")]
        public bool WebNoDotHtml { get; set; }

        /// <summary>
        /// The web interface parameter to run Vue.js application.
        /// </summary>
        [JsonProperty("webVueSupport")]
        public bool WebVueSupport { get; set; }
    }

    public class ReNoteConfig
    {
        /// <summary>
        /// The location of the database file.
        /// </summary>
        [JsonProperty("dbLocation")]
        public string DBLocation { get; set; }

        /// <summary>
        /// The folder location that stores backups of the database.
        /// </summary>
        [JsonProperty("dbBackupLocation")]
        public string DBBackupLocation { get; set; }

        /// <summary>
        /// The name of the school.
        /// </summary>
        [JsonProperty("schoolName")]
        public string SchoolName { get; set; }

        /// <summary>
        /// The type of school.
        /// </summary>
        [JsonProperty("schoolType")]
        public int SchoolType { get; set; }

        /// <summary>
        /// The location of the school.
        /// </summary>
        [JsonProperty("schoolLocation")]
        public string SchoolLocation { get; set; }
    }
}