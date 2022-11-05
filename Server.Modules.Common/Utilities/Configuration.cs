using System.Text.Json;

namespace Server.Common.Utilities
{
    public class Configuration
    {
        public static bool IsConfigurationsLoaded { get; private set; }
        public static GlobalConfig? GlobalConfig { get; set; }
        public static WebConfig? WebConfig { get; set; }

        public static void LoadAllConfigurations()
        {
            if (IsConfigurationsLoaded)
                return;

            GlobalConfig = LoadConfiguration<GlobalConfig>("global");
            WebConfig = LoadConfiguration<WebConfig>("web");

            IsConfigurationsLoaded = true;
        }

        public static T LoadConfiguration<T>(string name) where T : new()
        {
            T? config;
            string configFileName = $"{name.ToLower()}.config.json";

            if (!File.Exists(configFileName))
                return new T();

            string configContent = File.ReadAllText(configFileName);

            if (ValiditateJson(configContent))
                config = JsonSerializer.Deserialize<T>(configContent);
            else
                config = new T();

            config ??= new T();

            return config;
        }

        public static void SaveConfiguration<T>(string name, T configObject)
        {
            string data = JsonSerializer.Serialize(configObject);
            File.WriteAllText($"{name.ToLower()}.config.json", data);
        }

        public static bool ValiditateJson(string content)
        {
            if (string.IsNullOrEmpty(content))
                return false;

            try
            {
                JsonDocument.Parse(content);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }

    public class GlobalConfig
    {
        public string[] Prefixes { get; set; }
        public ushort WebPort { get; set; }
        public ushort ApiPort { get; set; }
        public ushort SocketPort { get; set; }

        public GlobalConfig() => Prefixes = Array.Empty<string>();
    }

    public class WebConfig
    {
        public string? WebRoot { get; set; }
        public bool WebNoDotHtml { get; set; }
        public bool WebVueSupport { get; set; }
    }
}