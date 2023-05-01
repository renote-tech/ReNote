using Client.ReNote.Data;
using Newtonsoft.Json;

namespace Client.Api.Responses
{
    internal class SchoolResponse : Response
    {
        [JsonProperty("data", Order = 10)]
        public School Data { get; set; }
    }
}