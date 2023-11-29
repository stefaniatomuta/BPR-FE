namespace BPR.Mediator.Interfaces;

public interface IHttpService
{
    Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest body);
}