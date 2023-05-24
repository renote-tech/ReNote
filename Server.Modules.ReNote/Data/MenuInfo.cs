using Newtonsoft.Json;

namespace Server.ReNote.Data
{
    public class MenuInfo
    {
        /// <summary>
        /// The ID of the <see cref="MenuInfo"/>.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        
        /// <summary>
        /// The name of the <see cref="MenuInfo"/>.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The icon of the <see cref="MenuInfo"/>.
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; }
    }
}