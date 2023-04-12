using Newtonsoft.Json;

namespace Server.ReNote.Data
{
    public class Theme
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("colorAccent")]
        public string ColorAccent { get; set; }

        [JsonProperty("isDarkTheme")]
        public bool IsDarkTheme { get; set; }
    }
}