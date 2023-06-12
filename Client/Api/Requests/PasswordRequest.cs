namespace Client.Api.Requests;

using Newtonsoft.Json;

internal class PasswordRequest : Request
{
    [JsonProperty("currentPassword")]
    public string CurrentPassword { get; set; }

    [JsonProperty("newPassword")]
    public string NewPassword { get; set; }

    public PasswordRequest(string currentPassword, string newPassword)
    {
        CurrentPassword = currentPassword;
        NewPassword = newPassword;
    }
}