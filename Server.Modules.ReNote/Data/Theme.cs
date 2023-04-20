using Newtonsoft.Json;

namespace Server.ReNote.Data
{
    public class Theme
    {
        /// <summary>
        /// The name of the <see cref="Theme"/>.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The color accent of the <see cref="Theme"/>.
        /// </summary>
        [JsonProperty("colorAccent")]
        public string ColorAccent { get; set; }

        /// <summary>
        /// Returns whether the <see cref="Theme"/> is a dark one or not.
        /// </summary>
        [JsonProperty("isDarkTheme")]
        public bool IsDarkTheme { get; set; }
    }
}