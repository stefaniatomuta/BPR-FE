﻿using BPR.Mediator.Interfaces.Messaging;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace BPR.Mediator.Services.Messaging;

public class RabbitMqConsumer<T> : RabbitMqBase, IConsumer<T>
{
    public event Func<T, Task>? MessageReceivedEvent;

    private const string queueName = "analysis_sender";
    private const int delay = 10000;

    public RabbitMqConsumer(ILogger<RabbitMqConsumer<T>> logger) : base(logger)
    {
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queueName, false, false, true);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var response = JsonSerializer.Deserialize<T>(message);

            _logger.LogDebug("Received message from queue: '{Queue}': '{Message}'", queueName, message);

            if (response != null)
            {
                MessageReceivedEvent?.Invoke(response);
            }
        };

        channel.BasicConsume(queueName, true, consumer);
        _logger.LogInformation("Waiting for messages from queue: '{Queue}'...", queueName);

        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(delay, cancellationToken);
        }
    }
}