namespace BPR.Mediator.Interfaces.Messaging;

public interface IConsumer<T>
{
    event Func<T, Task>? MessageReceivedEvent;
    Task ConsumeAsync(CancellationToken cancellationToken);
}
