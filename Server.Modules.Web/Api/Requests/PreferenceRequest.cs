using Newtonsoft.Json;

namespace Server.Web.Api.Requests
{
    internal class PreferenceRequest
    {
        /// <summary>
        /// The language of the <see cref="PreferenceRequest"/>.
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// The theme of the <see cref="PreferenceRequest"/>.
        /// </summary>
        [JsonProperty("theme")]
        public string Theme { get; set; }
    }
}