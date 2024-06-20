using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuotesApi.Models.Request;

public class UpdateUserRequestRm
{
    [JsonPropertyName("user")] public required UserInfoRm User { get; set; }

    string ToJson() => JsonSerializer.Serialize(this);
}