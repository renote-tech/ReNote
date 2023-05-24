namespace Client.Api.Responses;

using Newtonsoft.Json;

internal class Response
{
    [JsonProperty("status")]
    public int Status { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }
}