using System.Collections.Generic;
using Newtonsoft.Json;

namespace Server.ReNote.Data
{
    public class ToolbarInfo
    {
        /// <summary>
        /// The ID of the <see cref="ToolbarInfo"/>.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The name of the <see cref="ToolbarInfo"/>.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The default page of the <see cref="ToolbarInfo"/>.
        /// </summary>
        [JsonProperty("defaultPage")]
        public string DefaultPage { get; set; }

        /// <summary>
        /// The buttons of the <see cref="ToolbarInfo"/>.
        /// </summary>
        [JsonProperty("buttons")]
        public Dictionary<string, string> Buttons { get; set; }
    }
}