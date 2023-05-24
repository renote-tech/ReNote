namespace Client.Api.Responses;

using Client.ReNote.Data;
using Newtonsoft.Json;

internal class TeamProfileResponse : Response
{
    [JsonProperty("data", Order = 10)]
    public Team Data { get; set; }
}