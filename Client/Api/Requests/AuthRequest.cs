namespace Client.Api.Requests;

using Newtonsoft.Json;

internal class AuthRequest : Request
{
    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }

    public AuthRequest(string username, string password)
    {
        Username = username;
        Password = password;
    }
}