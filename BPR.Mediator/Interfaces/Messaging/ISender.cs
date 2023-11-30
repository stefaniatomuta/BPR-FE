using BPR.Model.Requests;

namespace BPR.Mediator.Interfaces.Messaging;

public interface ISender
{
    Task SendAsync<T>(T request);
}
