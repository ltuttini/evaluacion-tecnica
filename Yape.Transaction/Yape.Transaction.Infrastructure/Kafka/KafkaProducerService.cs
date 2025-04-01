using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Yape.Transaction.Infrastructure.Entity;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Yape.Transaction.Infrastructure.Kafka
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IProducer<Null, string> _produce;
        private readonly KafkaSettings _configuration;
        private readonly ILogger<KafkaProducerService> _logger;

        public KafkaProducerService(IProducer<Null, string> produce,
            IOptions<KafkaSettings> configuration, 
            ILogger<KafkaProducerService> logger)
        {
            _produce = produce;
            _configuration = configuration.Value;
            _logger = logger;
        }

        public async Task ProduceAsync(TransactionEntity mensaje)
        {
            try
            {
                string jsonMessage = JsonSerializer.Serialize(mensaje);
                var deliveryReport = await _produce.ProduceAsync(_configuration.Topic, new Message<Null, string> { Value = jsonMessage });

                _logger.LogInformation("Mensaje entregado a {0}", deliveryReport.Message);
            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError("Falló la entrega del mensaje: {0}", e.Error.Reason);
                throw;
            }
        }

    }
}
