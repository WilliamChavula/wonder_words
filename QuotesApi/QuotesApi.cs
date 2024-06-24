using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using QuotesApi.Models;
using QuotesApi.Models.Request;
using QuotesApi.Models.Response;

namespace QuotesApi;

public delegate Task<string?> UserTokenSupplier();

public class QuotesApi
{
    private const string ErrorCodeJsonKey = "error_code";
    private const string ErrorMessageJsonKey = "message";

    public QuotesApi(UserTokenSupplier userTokenSupplier, HttpClient? httpClient, UrlBuilder? urlBuilder)
    {
        _urlBuilder = urlBuilder ?? new UrlBuilder();

        _client = httpClient ??
                  new HttpClient(new RequestInterceptHandler(new HttpClientHandler(), userTokenSupplier));
        _client.SetupHeaders();
    }

    private readonly UrlBuilder _urlBuilder;
    private readonly HttpClient _client;

    public async Task<QuoteListPageRm> GetQuoteListPage(int page, string? tag, string? favoredByUsername,
        string searchTerm = "")
    {
        var url = _urlBuilder.BuildGetQuoteListPageUrl(page, tag, favoredByUsername, searchTerm);

        var response = await _client.GetFromJsonAsync<QuoteListPageRm>(url);

        var firstItem = response?.QuoteList.First();

        if (firstItem is { Id: 0 })
        {
            throw new EmptySearchResultQuoteException();
        }

        return response!;
    }

    public async Task<QuoteRm> GetQuote(int id)
    {
        var url = _urlBuilder.BuildGetQuoteUrl(id);
        var response = await _client.GetFromJsonAsync<QuoteRm>(url);

        return response!;
    }

    public Task<QuoteRm> FavoriteQuote(int id)
    {
        var url = _urlBuilder.BuildFavoriteQuoteUrl(id);
        return UpdateQuote(url);
    }

    public Task<QuoteRm> UnFavoriteQuote(int id)
    {
        var url = _urlBuilder.BuildUnfavoriteQuoteUrl(id);
        return UpdateQuote(url);
    }

    public Task<QuoteRm> UpVoteQuote(int id)
    {
        var url = _urlBuilder.BuildUpvoteQuoteUrl(id);
        return UpdateQuote(url);
    }

    public Task<QuoteRm> DownVoteQuote(int id)
    {
        var url = _urlBuilder.BuildDownvoteQuoteUrl(id);
        return UpdateQuote(url);
    }

    public Task<QuoteRm> UnVoteQuote(int id)
    {
        var url = _urlBuilder.BuildUnvoteQuoteUrl(id);
        return UpdateQuote(url);
    }

    private async Task<QuoteRm> UpdateQuote(string url)
    {
        using var response = await _client.PutAsync(url, null);
        var jsonResponse = await response.Content.ReadAsStringAsync();

        var jsonDict = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);

        if (jsonDict != null && jsonDict.TryGetValue(ErrorCodeJsonKey, out var code))
        {
            if ((int)code == 20)
            {
                throw new UserAuthRequiredQuoteException();
            }
        }

        var updatedQuote = JsonSerializer.Deserialize<QuoteRm>(jsonResponse);
        if (updatedQuote is null)
            throw new InvalidOperationException();

        return updatedQuote;
    }

    public async Task<UserRm> SignIn(string email, string password)
    {
        var url = _urlBuilder.BuildSignInUrl();
        var requestJsonBody = new SignInRequestRm
        {
            Credentials = new UserCredentialsRm
            {
                Email = email,
                Password = password
            }
        };

        using var response = await _client.PostAsJsonAsync(url, requestJsonBody);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var jsonDict = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);

        if (jsonDict is not null && jsonDict.TryGetValue(ErrorCodeJsonKey, out var code))
        {
            if ((int)code == 21)
            {
                throw new InvalidCredentialsQuoteException();
            }
        }

        var user = JsonSerializer.Deserialize<UserRm>(jsonResponse);
        if (user is null)
            throw new InvalidOperationException();

        return user;
    }

    public async Task<string> SignUp(string username, string email, string password)
    {
        var url = _urlBuilder.BuildSignUpUrl();
        var requestBody = new SignUpRequestRm
        {
            User = new UserInfoRm
            {
                Email = email,
                Password = password,
                Username = username
            }
        };

        using var response = await _client.PostAsJsonAsync(url, requestBody);
        var jsonResponse = await response.Content.ReadAsStringAsync();

        var jsonDict = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);

        if (jsonDict is not null && jsonDict.TryGetValue(ErrorCodeJsonKey, out var code))
        {
            if ((int)code == 32)
            {
                if (jsonDict[ErrorMessageJsonKey] is string errorMessage &&
                    errorMessage.Contains("email", StringComparison.CurrentCultureIgnoreCase))
                    throw new EmailAlreadyRegisteredQuoteException();

                throw new UsernameAlreadyTakenQuoteException();
            }
        }

        if (jsonDict is null)
            throw new InvalidOperationException();

        return (string)jsonDict["User-Token"];
    }

    public async Task UpdateProfile(string username, string email, string? password)
    {
        var url = _urlBuilder.BuildUpdateProfileUrl(username);
        var requestBody = new UpdateUserRequestRm
        {
            User = new UserInfoRm
            {
                Email = email,
                Password = password,
                Username = username
            }
        };

        using var response = await _client.PutAsJsonAsync(url, requestBody);
        
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var jsonDict = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);

        if (jsonDict is not null && jsonDict.TryGetValue(ErrorCodeJsonKey, out var code))
            if ((int)code == 32)
                throw new UsernameAlreadyTakenQuoteException();
    }

    public async Task SignOut()
    {
        var url = _urlBuilder.BuildSignOutUrl();
        await _client.DeleteAsync(url);
    }

    public async Task RequestPasswordResetEmail(string email)
    {
        var url = _urlBuilder.BuildRequestPasswordResetEmailUrl();
        using var response = await _client.PostAsJsonAsync(url, new PasswordResetEmailRequestRm
        {
            User = new UserEmailRm
            {
                Email = email
            }
        });
    }
}

public static class HttpClientExtension
{
    private const string AppTokenEnvironmentVariableKey = "quotes-app-token";

    public static void SetupHeaders(this HttpClient client)
    {
        var appToken = Environment.GetEnvironmentVariable(AppTokenEnvironmentVariableKey);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Token token={appToken}");
    }
}

public class RequestInterceptHandler(HttpMessageHandler innerHandler, UserTokenSupplier userToken)
    : DelegatingHandler(innerHandler)
{
    private UserTokenSupplier UserToken { get; } = userToken;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await UserToken();
        
        if(token is not null)
            request.Headers.Add("User-Token", token);
        
        return await base.SendAsync(request, cancellationToken);
    }
}