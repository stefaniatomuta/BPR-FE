using System.Text;
using System.Text.Json;
using BPR.Mediator.Interfaces;

namespace BPR.Mediator.Services;

public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public HttpService(IHttpClientFactory httpClientFactory, JsonSerializerOptions jsonSerializerOptions)
    {
        _httpClient = httpClientFactory.CreateClient();
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest body)
    {
        var content = JsonSerializer.Serialize(body, _jsonSerializerOptions);
        var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = new StringContent(content, Encoding.UTF8, "application/json")
        };

        var response = await _httpClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error - Status code: '{response.StatusCode}', Response body: '{json}'");
        }

        return JsonSerializer.Deserialize<TResponse>(json, _jsonSerializerOptions)!;
    }
}