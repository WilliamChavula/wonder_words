using Realms;

namespace LocalStorage.Models;

public partial class QuotesCm : EmbeddedObject
{
    public string Body { get; set; }
    public string? Author { get; set; }
    public int FavoriteCount { get; set; }
    public int UpVoteCount { get; set; }
    public int DownVoteCount { get; set; }
    public bool? IsFavorite { get; set; }
    public bool? IsUpVoted { get; set; }
    public bool? IsDownVoted { get; set; }

    public QuotesCm()
    {
    }

    public QuotesCm(int id, string body, string? author, int favoriteCount, int upVoteCount, int downVoteCount,
        bool? isFavorite, bool? isUpVoted, bool? isDownVoted)
    {
        Body = body;
        Author = author;
        FavoriteCount = favoriteCount;
        UpVoteCount = upVoteCount;
        DownVoteCount = downVoteCount;
        IsFavorite = isFavorite;
        IsUpVoted = isUpVoted;
        IsDownVoted = isDownVoted;
    }
}