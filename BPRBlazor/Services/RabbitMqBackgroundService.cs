using BPR.Mediator.Interfaces.Messaging;

namespace BPRBlazor.Services;

public class RabbitMqBackgroundService<T> : BackgroundService
{
    private readonly IConsumer<T> _messagingService;

    public RabbitMqBackgroundService(IConsumer<T> messagingService)
    {
        _messagingService = messagingService;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(5000);
        await _messagingService.ConsumeAsync(cancellationToken);
    }
}
