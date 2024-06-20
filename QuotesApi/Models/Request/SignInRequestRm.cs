using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuotesApi.Models.Request;

public class SignInRequestRm
{
    [JsonPropertyName("user")] public required UserCredentialsRm Credentials { get; set; }
}