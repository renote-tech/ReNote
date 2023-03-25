using Newtonsoft.Json;

namespace Server.Web.Api.Responses
{
    public class QuotationResponse
    {
        /// <summary>
        /// The author of the quotation.
        /// </summary>
        [JsonProperty("author")]
        public string Author { get; set; }

        /// <summary>
        /// The content of the quotation.
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}