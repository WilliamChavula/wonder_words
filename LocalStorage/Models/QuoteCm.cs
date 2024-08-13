using Realms;

namespace LocalStorage.Models;

public partial class QuoteCm : RealmObject
{
    [PrimaryKey]
    public int Id { get; set; }
    public required string Body { get; set; }
    public string? Author { get; set; }
    public int FavoriteCount { get; set; }
    public int UpVoteCount { get; set; }
    public int DownVoteCount { get; set; }
    public bool? IsFavorite { get; set; }
    public bool? IsUpVoted { get; set; }
    public bool? IsDownVoted { get; set; }
}