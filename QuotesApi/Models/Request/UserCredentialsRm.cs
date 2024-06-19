using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuotesApi.Models.Request;

public class UserCredentialsRm
{
    [JsonPropertyName("login")] public required string Email { get; set; }
    [JsonPropertyName("password")] public required string Password { get; set; }

    string ToJson() => JsonSerializer.Serialize(this);
}