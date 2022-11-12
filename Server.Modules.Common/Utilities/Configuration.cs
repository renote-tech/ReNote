using System.Text.Json;

namespace Server.Common.Utilities
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
        /// Loads all configurations including <see cref="GlobalConfig"/> and <see cref="WebConfig"/>.
        /// </summary>
        public static void LoadAllConfigurations()
        {
            if (IsConfigurationsLoaded)
                return;

            GlobalConfig = LoadConfiguration<GlobalConfig>("Global");
            WebConfig = LoadConfiguration<WebConfig>("Web");

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

            if (JsonUtil.ValiditateJson(configContent))
                config = JsonSerializer.Deserialize<T>(configContent);
            else
                config = new T();

            config ??= new T();

            Platform.Log($"Loaded {name} configuration", LogLevel.INFO);

            return config;
        }

        /// <summary>
        /// Saves a specific configuration.
        /// </summary>
        /// <typeparam name="T">The configuration type.</typeparam>
        /// <param name="name">The configuration name.</param>
        /// <param name="configObject">The configuration object.</param>
        public static void SaveConfiguration<T>(string name, T configObject)
        {
            string data = JsonSerializer.Serialize(configObject);
            File.WriteAllText($"{name.ToLower()}.config.json", data);
        }
    }

    public class GlobalConfig
    {
        /// <summary>
        /// The HTTP urls prefixes.
        /// </summary>
        public string[] Prefixes { get; set; }
        /// <summary>
        /// The web interface port.
        /// </summary>
        public ushort WebPort { get; set; }
        /// <summary>
        /// The API interface port.
        /// </summary>
        public ushort ApiPort { get; set; }
        /// <summary>
        /// The socket interface port.
        /// </summary>
        public ushort SocketPort { get; set; }

        public GlobalConfig() => Prefixes = Array.Empty<string>();
    }

    public class WebConfig
    {
        /// <summary>
        /// The relative root path of the web interface.
        /// </summary>
        public string WebRoot { get; set; }
        /// <summary>
        /// The web interface parameter to modify the behavior of the urls.
        /// </summary>
        public bool WebNoDotHtml { get; set; }
        /// <summary>
        /// The web interface parameter to run Vue.js application.
        /// </summary>
        public bool WebVueSupport { get; set; }
    }
}