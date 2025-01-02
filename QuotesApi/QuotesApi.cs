using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using QuotesApi.Models;
using QuotesApi.Models.Request;
using QuotesApi.Models.Response;

namespace QuotesApi;

public class QuotesApi
{
    private const string ErrorCodeJsonKey = "error_code";
    private const string ErrorMessageJsonKey = "message";

    public QuotesApi(UserTokenSupplier userTokenSupplier, IConfiguration configuration)
    {
        _configuration = configuration;

        var key = _configuration["ApiKeys:quotes-app-token"];
        ArgumentNullException.ThrowIfNull(
            key,
            "ApiKeys:quotes-app-token must be set in launchsettings.json"
        );

        _urlBuilder = new UrlBuilder();

        _client = new HttpClient(
            new RequestInterceptHandler(new HttpClientHandler(), userTokenSupplier)
        )
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        _client.SetupHeaders(apiKey: key);
    }

    private readonly UrlBuilder _urlBuilder;
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;

    public async Task<QuoteListPageRm> GetQuoteListPage(
        int page,
        string? tag,
        string? favoredByUsername,
        string searchTerm = ""
    )
    {
        var url = _urlBuilder.BuildGetQuoteListPageUrl(page, tag, favoredByUsername, searchTerm);
        var serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var response = await _client.GetFromJsonAsync<QuoteListPageRm>(url);
        // var response = JsonSerializer.Deserialize<QuoteListPageRm>(await LocalApi.GetAsync(), serializerOptions);

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
        var jsonResponse = await response.Content.ReadFromJsonAsync<Dictionary<string, object>?>();

        if (jsonResponse != null && jsonResponse.TryGetValue(ErrorCodeJsonKey, out var code))
        {
            var err_code = (JsonElement)code;
            var error_int = err_code.GetInt32();
            if (error_int == 20)
            {
                throw new UserAuthRequiredQuoteException();
            }
        }

        var updatedString = await response.Content.ReadAsStringAsync();
        var updatedQuote =
            JsonSerializer.Deserialize<QuoteRm>(updatedString)
            ?? throw new InvalidOperationException();
        return updatedQuote;
    }

    public async Task<UserRm> SignIn(string email, string password)
    {
        var url = _urlBuilder.BuildSignInUrl();
        var requestJsonBody = new SignInRequestRm
        {
            Credentials = new UserCredentialsRm { Email = email, Password = password }
        };

        var jsonString = JsonSerializer.Serialize(requestJsonBody);
        HttpContent content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

        using var response = await _client.PostAsync(url, content);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var jsonDict = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);

        if (jsonDict is not null && jsonDict.TryGetValue(ErrorCodeJsonKey, out var code))
        {
            var err_code = (JsonElement)code;
            var error_int = err_code.GetInt32();
            if (error_int == 21)
            {
                throw new UserAuthRequiredQuoteException();
            }
        }

        var user = JsonSerializer.Deserialize<UserRm>(jsonResponse) ?? throw new InvalidOperationException();
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
            var err_code = (JsonElement)code;
            var error_int = err_code.GetInt32();
  
            if (error_int == 32)
            {
                if (
                    ((JsonElement)jsonDict[ErrorCodeJsonKey]).GetString() is string errorMessage
                    && errorMessage.Contains("email", StringComparison.CurrentCultureIgnoreCase)
                )
                    throw new EmailAlreadyRegisteredQuoteException();

                throw new UsernameAlreadyTakenQuoteException();
            }
        }

        if (jsonDict is null)
            throw new InvalidOperationException();
        

        return ((JsonElement)jsonDict["User-Token"]).GetString()!;
    }

    public async Task UpdateProfile(string username, string email, string password)
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
        {
            var err_code = (JsonElement)code;

            if (err_code.GetInt32() == 32)
                throw new UsernameAlreadyTakenQuoteException();
            
        }
    }

    public async Task SignOut()
    {
        var url = _urlBuilder.BuildSignOutUrl();
        await _client.DeleteAsync(url);
    }

    public async Task RequestPasswordResetEmail(string email)
    {
        var url = _urlBuilder.BuildRequestPasswordResetEmailUrl();
        using var response = await _client.PostAsJsonAsync(
            url,
            new PasswordResetEmailRequestRm { User = new UserEmailRm { Email = email } }
        );
    }
}

public static class HttpClientExtension
{
    private const string AppTokenEnvironmentVariableKey = "quotes-app-token";

    public static void SetupHeaders(this HttpClient client, string apiKey)
    {
        // var appToken = Environment.GetEnvironmentVariable(AppTokenEnvironmentVariableKey);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Token",
            $"token=\"{apiKey}\""
        );
    }
}

public class RequestInterceptHandler(HttpMessageHandler innerHandler, UserTokenSupplier userToken)
    : DelegatingHandler(innerHandler)
{
    private UserTokenSupplier UserToken { get; } = userToken;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        var token = await UserToken();

        if (token is not null)
            request.Headers.Add("User-Token", token);

        return await base.SendAsync(request, cancellationToken);
    }
}
