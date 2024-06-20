using System.Net.Http.Headers;

namespace QuotesApi;

public delegate Task<string?> UserTokenSupplier();

public class QuotesApi
{
    const string _errorCodeJsonKey = "error_code";
    const string _errorMessageJsonKey = "message";

    public QuotesApi(UserTokenSupplier userTokenSupplier, HttpClient? httpClient, UrlBuilder? urlBuilder)
    {
        _urlBuilder = urlBuilder ?? new UrlBuilder();

        var client = httpClient ??
                     new HttpClient(new RequestInterceptHandler(new HttpClientHandler(), userTokenSupplier));
        client.SetupHeaders();
    }

    private UrlBuilder _urlBuilder;
}

public static class HttpClientExtension
{
    private const string AppTokenEnvironmentVariableKey = "fav-qs-app-token";

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
        request.Headers.Add("User-Token", token);
        return await base.SendAsync(request, cancellationToken);
    }
}