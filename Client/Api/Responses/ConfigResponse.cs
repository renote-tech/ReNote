namespace Client.Api.Responses;

using System.Collections.Generic;
using Client.ReNote.Data;
using Newtonsoft.Json;

internal class ConfigResponse : Response
{
    [JsonProperty("data", Order = 10)]
    public ConfigData Data { get; set; }
}

internal class ConfigData
{
    [JsonProperty("menuInfo")]
    public MenuInfo[] MenuInfo { get; set; }

    [JsonProperty("features")]
    public Dictionary<string, bool> Features { get; set; }

    [JsonProperty("toolbarsInfo")]
    public ToolbarInfo[] ToolbarsInfo { get; set; }

    [JsonProperty("themes")]
    public Theme[] Themes { get; set; }
}

internal class ToolbarInfo
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("defaultPage")]
    public string DefaultPage { get; set; }

    [JsonProperty("buttons")]
    public Dictionary<string, string> Buttons { get; set; }
}

public class MenuInfo
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("icon")]
    public string Icon { get; set; }
}