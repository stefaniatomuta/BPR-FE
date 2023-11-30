using BPR.Mediator.Interfaces.Messaging;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace BPR.Mediator.Services.Messaging;

public class RabbitMqConsumer<T> : RabbitMqBase, IConsumer<T>
{
    private readonly JsonSerializerOptions _serializerOptions;

    public event Func<T, Task>? MessageReceivedEvent;

    private const string QueueName = "analysis_sender";
    private const int Delay = 10000;

    public RabbitMqConsumer(ILogger<RabbitMqConsumer<T>> logger, JsonSerializerOptions serializerOptions) : base(logger)
    {
        _serializerOptions = serializerOptions;
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(QueueName, false, false, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogDebug("Received message from queue: '{Queue}': '{Message}'", QueueName, message);
                var response = JsonSerializer.Deserialize<T>(message, _serializerOptions);

                if (response != null)
                {
                    MessageReceivedEvent?.Invoke(response);
                }
            };

            channel.BasicConsume(QueueName, true, consumer);
            _logger.LogInformation("Waiting for messages from queue: '{Queue}'...", QueueName);

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(Delay, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning("{ExceptionMessage}. Exception: {Exception}", ex.Message, ex);
        }
    }
}
