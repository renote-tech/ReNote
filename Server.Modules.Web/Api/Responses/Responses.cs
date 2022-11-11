using Newtonsoft.Json;

namespace Server.Web.Api.Responses
{
    internal class BasicResponse
    {
        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    internal class DataResponse : BasicResponse
    {
        [JsonProperty("data", Order = 10)]
        public object Data { get; set; }
    }
}