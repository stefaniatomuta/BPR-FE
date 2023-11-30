namespace BPR.Mediator.Interfaces.Messaging;

public interface ISender
{
    Task SendAsync<T>(T request);
}
