using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuotesApi.Models.Request;

public class UserEmailRm
{
    [JsonPropertyName("email")] public required string Email { get; set; }

    string ToJson() => JsonSerializer.Serialize(this);
}