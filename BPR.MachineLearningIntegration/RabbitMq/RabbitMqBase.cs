﻿using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace BPR.MachineLearningIntegration.RabbitMq;

public abstract class RabbitMqBase
{
    protected readonly ConnectionFactory _connectionFactory;
    protected readonly ILogger _logger;

    protected const string Endpoint = "amqp://guest:guest@localhost:5672/";

    protected RabbitMqBase(ILogger logger)
    {
        _connectionFactory = new ConnectionFactory
        {
            Uri = new Uri(Endpoint)
        };

        _logger = logger;
    }
}
