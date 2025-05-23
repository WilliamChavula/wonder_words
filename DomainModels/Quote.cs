namespace DomainModels;

public record Quote
{
    public int Id { get; set; }
    public required string Body { get; set; }
    public string? Author { get; set; }
    public bool? IsFavorite { get; set; }
    public bool? IsUpVoted { get; set; }
    public bool? IsDownVoted { get; set; }
    public int FavoriteCount { get; set; }
    public int UpVotesCount { get; set; }
    public int DownVotesCount { get; set; }
}