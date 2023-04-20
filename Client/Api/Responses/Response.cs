using Newtonsoft.Json;

namespace Client.Api.Responses
{
    internal class Response
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}