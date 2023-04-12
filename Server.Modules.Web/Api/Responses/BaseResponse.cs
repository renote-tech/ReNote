using Newtonsoft.Json;

namespace Server.Web.Api.Responses
{
    internal class Response
    {
        /// <summary>
        /// The status of the <see cref="Response"/>.
        /// </summary>
        [JsonProperty("status")]
        public long Status { get; set; }

        /// <summary>
        /// The message of the <see cref="Response"/>.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    internal class DataResponse : Response
    {
        /// <summary>
        /// The data of the <see cref="DataResponse"/>.
        /// </summary>
        [JsonProperty("data", Order = 10)]
        public object Data { get; set; }
    }
}