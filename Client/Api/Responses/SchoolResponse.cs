using Newtonsoft.Json;
using Client.ReNote;

namespace Client.Api.Responses
{
    internal class SchoolResponse : BaseResponse
    {
        [JsonProperty("data", Order = 10)]
        public School Data { get; set; }
    }
}