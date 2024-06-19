using System.Text.Json.Serialization;

namespace QuotesApi.Models.Response;

public class QuoteUserInfoRm
{
    [JsonPropertyName("favorite")] public required bool IsFavorite { get; set; }

    [JsonPropertyName("upvote")] public required bool IsUpVoted { get; set; }

    [JsonPropertyName("downvote")] public required bool IsDownVoted { get; set; }
}