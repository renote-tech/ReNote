using Client.ReNote;
using Newtonsoft.Json;

namespace Client.Api.Responses
{
    internal class ThemeResponse : Response
    {
        [JsonProperty("data", Order = 10)]
        public Theme[] Data { get; set; }
    }
}