namespace BPRBlazor.Services;

public interface IHttpService
{
    Task PostAsync(string endpoint, object body);
}