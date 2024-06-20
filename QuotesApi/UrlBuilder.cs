namespace QuotesApi;

public class UrlBuilder(string baseUrl = "https://favqs.com/api")
{
    private string BaseUrl { get; } = baseUrl;

    public string BuildGetQuoteListPageUrl(int page, string? tag, string? favoredByUsername, string searchTerm = "")
    {
        var condition = (string.IsNullOrEmpty(tag) && string.IsNullOrEmpty(searchTerm)) ||
                        (string.IsNullOrEmpty(searchTerm) && string.IsNullOrEmpty(favoredByUsername)) ||
                        (string.IsNullOrEmpty(favoredByUsername) && string.IsNullOrEmpty(tag));

        const string message = "FavQs doesn't support filtering favorites or searching by both query and 'tag at the same time.";
        FilterAssertion.Assert(condition, message);

        var tagQueryStringPart = tag is not null ? "&filter=$tag&type=tag" : "";
        var favoriteQueryStringPart = favoredByUsername is not null ? $"&filter={favoredByUsername}&type=user" : "";
        var searchQueryStringPart = !string.IsNullOrEmpty(searchTerm) ? $"&filter={searchTerm}" : "";
        
        return $"{BaseUrl}/quotes/?page={page}{tagQueryStringPart}{searchQueryStringPart}{favoriteQueryStringPart}";
    }
    
    public string BuildGetQuoteUrl(int id) {
        return $"{BaseUrl}/quotes/{id}";
    }

    public string BuildFavoriteQuoteUrl(int id) {
        return $"{BaseUrl}/quotes/{id}/fav";
    }

    public string BuildUnfavoriteQuoteUrl(int id) {
        return $"{BaseUrl}/quotes/{id}/unfav";
    }

    public string BuildUpvoteQuoteUrl(int id) {
        return $"{BaseUrl}/quotes/{id}/upvote";
    }

    public string BuildDownvoteQuoteUrl(int id) {
        return $"{BaseUrl}/quotes/{id}/downvote";
    }

    public string BuildUnvoteQuoteUrl(int id) {
        return $"{BaseUrl}/quotes/{id}/clearvote";
    }

    public string BuildSignInUrl() {
        return $"{BaseUrl}/session";
    }

    public string BuildSignOutUrl() {
        return $"{BaseUrl}/session";
    }

    public string BuildSignUpUrl() {
        return $"{BaseUrl}/users";
    }

    public string BuildUpdateProfileUrl(string username) {
        return $"{BaseUrl}/users/{username}";
    }

    public string BuildRequestPasswordResetEmailUrl() {
        return $"{BaseUrl}/users/forgot_password";
    }
}

static class FilterAssertion
{
    public static void Assert(bool condition, string? message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}