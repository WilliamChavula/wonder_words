using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuotesApi.Models.Request;

public class SignUpRequestRm
{
    [JsonPropertyName("user")] public required UserInfoRm User { get; set; }
}