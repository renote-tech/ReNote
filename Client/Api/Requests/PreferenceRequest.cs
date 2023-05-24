namespace Client.Api.Requests;

using Newtonsoft.Json;

internal class PreferenceRequest : Request
{
    [JsonProperty("language")]
    public string Language { get; set; }

    [JsonProperty("theme")]
    public string Theme { get; set; }

    public PreferenceRequest(string language, string theme)
    {
        Language = language;
        Theme = theme;
    }
}