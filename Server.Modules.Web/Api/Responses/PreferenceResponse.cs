using Newtonsoft.Json;

namespace Server.Web.Api.Responses
{
    internal class PreferenceResponse
    {
        /// <summary>
        /// The prefered language by the user.
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// The prefered theme by the user.
        /// </summary>
        [JsonProperty("theme")]
        public string Theme { get; set; }
    }
}