namespace Client.Api.Responses;

using Client.ReNote.Data;
using Newtonsoft.Json;

internal class SchoolResponse : Response
{
    [JsonProperty("data", Order = 10)]
    public School Data { get; set; }
}