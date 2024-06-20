namespace QuotesApi;

public class UrlBuilder(string baseUrl = "https://favqs.com/api")
{
    private string BaseUrl { get; } = baseUrl;

    string BuildGetQuoteListPageUrl(int page, string? tag, string? favoredByUsername, string searchTerm = "")
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
    
    string buildGetQuoteUrl(int id) {
        return $"{BaseUrl}/quotes/{id}";
    }

    string buildFavoriteQuoteUrl(int id) {
        return $"{BaseUrl}/quotes/{id}/fav";
    }

    string buildUnfavoriteQuoteUrl(int id) {
        return $"{BaseUrl}/quotes/{id}/unfav";
    }

    string buildUpvoteQuoteUrl(int id) {
        return $"{BaseUrl}/quotes/{id}/upvote";
    }

    string buildDownvoteQuoteUrl(int id) {
        return $"{BaseUrl}/quotes/{id}/downvote";
    }

    string buildUnvoteQuoteUrl(int id) {
        return $"{BaseUrl}/quotes/{id}/clearvote";
    }

    string buildSignInUrl() {
        return $"{BaseUrl}/session";
    }

    string buildSignOutUrl() {
        return $"{BaseUrl}/session";
    }

    string buildSignUpUrl() {
        return $"{BaseUrl}/users";
    }

    string buildUpdateProfileUrl(string username) {
        return $"{BaseUrl}/users/{username}";
    }

    string buildRequestPasswordResetEmailUrl() {
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