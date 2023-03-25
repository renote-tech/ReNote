using Newtonsoft.Json;

namespace Server.Web.Api.Responses
{
    internal class BasicResponse
    {
        /// <summary>
        /// The status of the <see cref="BasicResponse"/>.
        /// </summary>
        [JsonProperty("status")]
        public long Status { get; set; }

        /// <summary>
        /// The message of the <see cref="BasicResponse"/>.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    internal class DataResponse : BasicResponse
    {
        /// <summary>
        /// The data of the <see cref="DataResponse"/>.
        /// </summary>
        [JsonProperty("data", Order = 10)]
        public object Data { get; set; }
    }
}