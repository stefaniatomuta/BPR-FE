using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace BPR.Mediator.Services.Messaging;

public abstract class RabbitMqBase
{
    protected readonly ConnectionFactory _connectionFactory;
    protected readonly ILogger _logger;

    private const string endpoint = "amqp://guest:guest@localhost:5672/";

    protected RabbitMqBase(ILogger logger)
    {
        _connectionFactory = new ConnectionFactory
        {
            Uri = new Uri(endpoint)
        };

        _logger = logger;
    }
}
