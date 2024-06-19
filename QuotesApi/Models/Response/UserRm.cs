using System.Text.Json.Serialization;

namespace QuotesApi.Models.Response;

public class UserRm
{
    [JsonPropertyName("User-Token")] public required string Token { get; set; }
    
    [JsonPropertyName("login")] public required string Username { get; set; }
    
    [JsonPropertyName("email")] public required string Email { get; set; }
}