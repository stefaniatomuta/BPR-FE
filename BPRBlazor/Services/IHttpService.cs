namespace BPRBlazor.Services;

public interface IHttpService
{
    Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest body);
}