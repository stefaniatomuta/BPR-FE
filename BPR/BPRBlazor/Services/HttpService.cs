namespace BPRBlazor.Services;

public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public HttpService(IHttpClientFactory clientFactory, JsonSerializerOptions jsonSerializerOptions)
    {
        _httpClient = clientFactory.CreateClient();
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public async Task PostAsync(string endpoint, object body)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            endpoint);
        request.Content = JsonContent.Create(body);
        var response = await _httpClient.SendAsync(request);
    }
}