using System.Text.Json.Serialization;

namespace QuotesApi.Models.Response;

public class QuoteRm
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("body")] public string? Body { get; set; }

    [JsonPropertyName("author")] public string? Author { get; set; }

    [JsonPropertyName("user_details")] public QuoteUserInfoRm UserInfo { get; set; } = new()
    {
        IsFavorite = false,
        IsUpVoted = false,
        IsDownVoted = false
    };

    [JsonPropertyName("favorites_count")] public int FavoritesCount { get; set; }

    [JsonPropertyName("upvotes_count")] public int UpVotesCount { get; set; }

    [JsonPropertyName("downvotes_count")] public int DownVotesCount { get; set; }
}