using Client.ReNote.Data;
using Newtonsoft.Json;

namespace Client.Api.Responses
{
    internal class TeamProfileResponse : Response
    {
        [JsonProperty("data", Order = 10)]
        public Team Data { get; set; }
    }
}