using BPR.MachineLearningIntegration.Models;
using BPR.Mediator.Interfaces.Messaging;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using BPR.MachineLearningIntegration.Utils;
using BPR.Model.Architectures;

namespace BPR.MachineLearningIntegration.RabbitMq;

public class RabbitMqSender : RabbitMqBase, ISender
{
    private readonly JsonSerializerOptions _serializerOptions;

    private const string QueueName = "analysis_consumer";

    public RabbitMqSender(ILogger<RabbitMqSender> logger, JsonSerializerOptions serializerOptions) : base(logger)
    {
        _serializerOptions = serializerOptions;
    }

    public bool Send(string folderPath, List<Rule> rules, Guid correlationId)
    {
        try
        {
            var externalRules = ExternalRulesHelper.ToExternalAnalysisRules(rules);

            if (!externalRules.Any())
            {
                return false;
            }
            
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(QueueName, false, false, false);

            var request = new MLAnalysisRequestModel(folderPath, externalRules, correlationId);
            var message = JsonSerializer.Serialize(request, _serializerOptions);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(string.Empty, QueueName, null, body);
            _logger.LogDebug("Sent message to queue: '{Queue}'", QueueName);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning("{ExceptionMessage}. Exception: {Exception}", ex.Message, ex);
            throw;
        }
    }
}
