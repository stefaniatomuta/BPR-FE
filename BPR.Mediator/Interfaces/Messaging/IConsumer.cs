using BPR.Model.Results.External;

namespace BPR.Mediator.Interfaces.Messaging;

public interface IConsumer
{
    event Func<ExtendedAnalysisResults, Task>? MessageReceivedEvent;
    Task ConsumeAsync(CancellationToken cancellationToken);
}
