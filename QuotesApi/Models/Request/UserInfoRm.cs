using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuotesApi.Models.Request;

public class UserInfoRm
{
    [JsonPropertyName("login")] public required string Username { get; set; }
    [JsonPropertyName("email")] public required string Email { get; set; }
    [JsonPropertyName("password")] public required string Password { get; set; }
}