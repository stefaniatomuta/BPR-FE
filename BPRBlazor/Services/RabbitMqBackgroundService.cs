using BPR.Mediator.Interfaces.Messaging;

namespace BPRBlazor.Services;

public class RabbitMqBackgroundService : BackgroundService
{
    private readonly IConsumer _messagingService;

    public RabbitMqBackgroundService(IConsumer messagingService)
    {
        _messagingService = messagingService;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(5000, cancellationToken);
        await _messagingService.ConsumeAsync(cancellationToken);
    }
}
