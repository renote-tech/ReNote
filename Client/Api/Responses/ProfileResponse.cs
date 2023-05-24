using Newtonsoft.Json;

namespace Client.Api.Responses;

internal class ProfileResponse : Response
{
    [JsonProperty("data", Order = 10)]
    public ProfileData Data { get; set; }
}

internal class ProfileData
{
    [JsonProperty("realName")]
    public string RealName { get; set; }

    [JsonProperty("teamId")]
    public int TeamId { get; set; }

    [JsonProperty("profilePicture")]
    public string ProfilePicture { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("phone")]
    public string PhoneNumber { get; set; }

    [JsonProperty("birthday")]
    public string Birthday { get; set; }

    [JsonProperty("lastConnection")]
    public long LastConnection { get; set; }
}