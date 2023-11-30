using BPR.Mediator.Interfaces;
using BPR.Model.Requests;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace BPR.Mediator.Services.Messaging;

public class RabbitMqSender : RabbitMqBase, ISender
{
    private const string queueName = "analysis_consumer";

    public RabbitMqSender(ILogger<RabbitMqSender> logger) : base(logger)
    {
    }

    public Task SendAsync(MLAnalysisRequestModel request)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queueName, false, false, true);

        var message = JsonSerializer.Serialize(request);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(string.Empty, queueName, null, body);
        _logger.LogDebug("Sent message to queue: '{Queue}'", queueName);

        return Task.CompletedTask;
    }
}
