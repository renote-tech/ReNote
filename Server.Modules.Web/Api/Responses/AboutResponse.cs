using Newtonsoft.Json;

namespace Server.Web.Api.Responses
{
    internal class AboutResponse
    {
        /// <summary>
        /// The name of the application.
        /// </summary>
        [JsonProperty("softwareName")]
        public string SoftwareName { get; set; }

        /// <summary>
        /// The version of the application.
        /// </summary>
        [JsonProperty("softwareVersion")]
        public string SoftwareVersion { get; set; }

        /// <summary>
        /// The application copyright.
        /// </summary>
        [JsonProperty("softwareCopyright")]
        public string SoftwareCopyright { get; set; }
    }
}