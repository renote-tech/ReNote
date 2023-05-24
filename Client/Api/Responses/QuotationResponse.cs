namespace Client.Api.Responses;

using Newtonsoft.Json;

internal class QuotationResponse : Response
{
    [JsonProperty("data", Order = 10)]
    public QuotationData Data { get; set; }
}

internal class QuotationData
{
    [JsonProperty("author")]
    public string Author { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }
}