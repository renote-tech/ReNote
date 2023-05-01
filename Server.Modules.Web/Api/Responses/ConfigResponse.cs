using Newtonsoft.Json;
using Server.ReNote.Data;
using System.Collections.Generic;

namespace Server.Web.Api.Responses
{
    internal class ConfigResponse
    {
        /// <summary>
        /// The features of the <see cref="ConfigResponse"/>.
        /// </summary>
        [JsonProperty("features")]
        public Dictionary<string, bool> Features { get; set; }

        /// <summary>
        /// The toolbars information of the <see cref="ConfigResponse"/>.
        /// </summary>
        [JsonProperty("toolbarsInfo")]
        public ToolbarInfo[] ToolbarsInfo { get; set; }

        /// <summary>
        /// The themes of the <see cref="ConfigResponse"/>.
        /// </summary>
        [JsonProperty("themes")]
        public Theme[] Themes { get; set; }
    }
}