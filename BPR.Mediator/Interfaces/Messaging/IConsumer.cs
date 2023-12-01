using BPR.Model.Results;

namespace BPR.Mediator.Interfaces.Messaging;

public interface IConsumer
{
    event Func<ExtendedAnalysisResults, Task>? MessageReceivedEvent;
    Task ConsumeAsync(CancellationToken cancellationToken);
}
