using Newtonsoft.Json;

namespace Client.Api.Responses
{
    internal class PreferenceResponse : Response
    {
        [JsonProperty("data", Order = 10)]
        public PreferenceData Data { get; set; }
    }

    internal class PreferenceData
    {
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("theme")]
        public string Theme { get; set; }
    }
}