using AutoMapper;
using BPR.MachineLearningIntegration.Backoff;
using BPR.MachineLearningIntegration.Models;
using BPR.Mediator.Interfaces.Messaging;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text;
using System.Text.Json;
using BPR.Model.Results;

namespace BPR.MachineLearningIntegration.RabbitMq;

public class RabbitMqConsumer : RabbitMqBase, IConsumer
{
    private readonly IMapper _mapper;
    private readonly JsonSerializerOptions _serializerOptions;

    public event Func<ExtendedAnalysisResults, Task>? MessageReceivedEvent;

    private const string QueueName = "analysis_sender";
    private const int Delay = 10000;
    private readonly ExponentialBackoff _backoffStrategy;

    public RabbitMqConsumer(ILogger<RabbitMqConsumer> logger, IMapper mapper, JsonSerializerOptions serializerOptions) : base(logger)
    {
        _mapper = mapper;
        _serializerOptions = serializerOptions;
        _backoffStrategy = new(TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(60));
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(QueueName, false, false, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (_, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogDebug("Received message from queue: '{Queue}': '{Message}'", QueueName, message);
                var response = JsonSerializer.Deserialize<MLAnalysisResponseModel>(message, _serializerOptions);
                var result = _mapper.Map<ExtendedAnalysisResults>(response);

                if (result != null)
                {
                    MessageReceivedEvent?.Invoke(result);
                }
            };

            channel.BasicConsume(QueueName, true, consumer);
            _logger.LogInformation("Connection established! Consuming messages from queue: '{Queue}'...", QueueName);

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(Delay, cancellationToken);
            }
        }
        catch (BrokerUnreachableException ex)
        {
            _logger.LogWarning("{ExceptionMessage}. Retrying in {BackoffTime} seconds...", ex.Message, _backoffStrategy.Backoff.TotalSeconds);
            await _backoffStrategy.PerformBackoff(cancellationToken);
            await ConsumeAsync(cancellationToken);
        }
    }
}
