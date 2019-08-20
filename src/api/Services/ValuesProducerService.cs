using System;
using System.Threading;
using System.Threading.Tasks;
using api.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace api.Services
{
    public class ValuesProducerService : BackgroundService
    {
        private readonly ProducerConfig _config;
        private readonly ILogger _logger;
        private string _topicName;
        private string _UUID;

        public ValuesProducerService(ILogger<ValuesProducerService> logger, ProducerConfig producerConfig, SettingsConfig settingsConfig)
        {
            _topicName = settingsConfig.TopicName;
            _UUID = settingsConfig.UUID;
            _config = producerConfig;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var _producer = new ProducerBuilder<Null, string>(_config).Build())
                {
                    try
                    {
                        var message = new
                        {
                            UUID = _UUID,
                            Id = new Random().Next(100),
                            Message = "Hello World",
                            Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()
                        };

                        await _producer.ProduceAsync(_topicName, new Message<Null, string>
                        {
                            Value = message.ToString(),
                            Timestamp = new Timestamp()
                        });
                    }
                    catch (ProduceException<Null, string> e)
                    {
                        _logger.LogInformation($"DeliveryResult failed: {e.Error.Reason}");
                    }
                }

                DateTime nextStop = DateTime.Now.AddSeconds(5);
                var timeToWait = nextStop - DateTime.Now;
                var millisToWait = timeToWait.TotalMilliseconds;
                Thread.Sleep((int)millisToWait);
            }
        }
    }
}
