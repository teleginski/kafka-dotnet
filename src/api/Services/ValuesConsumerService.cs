using System;
using System.Threading;
using System.Threading.Tasks;
using api.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace api.Services
{
    public class ValuesConsumerService : BackgroundService
    {
        private readonly ConsumerConfig _config;
        private readonly ILogger _logger;
        private string _topicName;

        public ValuesConsumerService(ILogger<ValuesConsumerService> logger, ConsumerConfig consumerConfig, SettingsConfig settingsConfig)
        {
            _topicName = settingsConfig.TopicName;
            _config = consumerConfig;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_config).Build())
            {
                consumer.Subscribe(_topicName);

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var consumeResult = consumer.Consume(cts.Token);
                            Console.WriteLine($"Consumed message '{consumeResult.Value}'");
                            _logger.LogInformation($"Consumed message '{consumeResult.Value}'");
                        }
                        catch (ConsumeException e)
                        {
                            _logger.LogError($"ConsumeResult Error: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }
            }
        }
    }
}