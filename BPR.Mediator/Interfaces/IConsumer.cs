namespace BPR.Mediator.Interfaces;

public interface IConsumer
{
    Task ConsumeAsync(CancellationToken cancellationToken);
}
