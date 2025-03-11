using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Yape.Transaction.Infrastructure.Entity;

namespace Yape.Transaction.Infrastructure.Kafka
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly KafkaSettings _configuration;

        public KafkaProducerService(IOptions<KafkaSettings> configuration)
        {
            _configuration = configuration.Value;
        }

        public async Task ProduceAsync(TransactionEntity mensaje)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _configuration.BootstrapServers
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    string jsonMessage = JsonSerializer.Serialize(mensaje);
                    var deliveryReport = await producer.ProduceAsync(_configuration.Topic, new Message<Null, string> { Value = jsonMessage });
                    
                    Console.WriteLine($"Mensaje entregado a {deliveryReport.TopicPartitionOffset}");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Falló la entrega del mensaje: {e.Error.Reason}");
                }
            }
        }

    }
}
