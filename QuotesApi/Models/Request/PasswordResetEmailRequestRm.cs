using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuotesApi.Models.Request;

public class PasswordResetEmailRequestRm
{
    [JsonPropertyName("user")] public required UserEmailRm User { get; set; }
}