using BPR.Mediator.Interfaces;
using BPR.Mediator.Services.Messaging;

namespace BPRBlazor.Services;

public class RabbitMqBackgroundService : BackgroundService
{
    private readonly IConsumer _messagingService;

    public RabbitMqBackgroundService(RabbitMqConsumer messagingService)
    {
        _messagingService = messagingService;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await _messagingService.ConsumeAsync(cancellationToken);
    }
}
