using BPR.Mediator.Interfaces.Messaging;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace BPR.Mediator.Services.Messaging;

public class RabbitMqSender : RabbitMqBase, ISender
{
    private readonly JsonSerializerOptions _serializerOptions;

    private const string queueName = "analysis_consumer";

    public RabbitMqSender(ILogger<RabbitMqSender> logger, JsonSerializerOptions serializerOptions) : base(logger)
    {
        _serializerOptions = serializerOptions;
    }

    public Task SendAsync<T>(T request)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queueName, false, false, true);

            var message = JsonSerializer.Serialize(request, _serializerOptions);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(string.Empty, queueName, null, body);
            _logger.LogDebug("Sent message to queue: '{Queue}'", queueName);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("{ExceptionMessage}. Exception: {Exception}", ex.Message, ex);
        }
        
        return Task.CompletedTask;
    }
}
