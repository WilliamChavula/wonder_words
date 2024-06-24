using System.Text.Json.Serialization;

namespace QuotesApi.Models.Response;

public class QuoteListPageRm
{
    [JsonPropertyName("last_page")] public required bool IsLastPage { get; set; }
    
    [JsonPropertyName("last_page")] public int? PageNumber { get; set; }
    [JsonPropertyName("quotes")] public required IList<QuoteRm> QuoteList { get; set; }
}