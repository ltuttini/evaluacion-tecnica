using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Transactions;
using Yape.AntiFraud.Core.Entity;
using Yape.AntiFraud.Core.Settings;
using Yape.AntiFraud.Strategy;

namespace Yape.AntiFraud.Core
{

    public class KafkaConsumerService : BackgroundService
    {
        private readonly KafkaSettings _kafkaSettings;
        private readonly TransactionSettings _transactionSettings;
        private readonly HttpClient _clientHttp;
        private readonly ILogger<KafkaConsumerService> _logger;
        private readonly AntiFraudStrategyFactory _antiFraudStrategy;

        public KafkaConsumerService(IOptions<KafkaSettings> kafkaSettings,
            IOptions<TransactionSettings> transactionSettings,
            IHttpClientFactory clientFactory,
            AntiFraudStrategyFactory antiFraudStrategy,
            ILogger<KafkaConsumerService> logger)
        {
            _kafkaSettings = kafkaSettings.Value;
            _transactionSettings = transactionSettings.Value;
            _clientHttp = clientFactory.CreateClient();
            _logger = logger;
            _antiFraudStrategy = antiFraudStrategy;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _kafkaSettings.BootstrapServers,
                GroupId = _kafkaSettings.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest // O Latest, dependiendo de tus necesidades
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(_kafkaSettings.Topic);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        // Se obtiene el mensaje desde la queue
                        //
                        var consumeResult = consumer.Consume(stoppingToken);
                        _logger.LogInformation("Mensaje recibido: {0}", consumeResult.Message.Value);

                        var message = JsonSerializer.Deserialize<TransactionMessage>(consumeResult.Message.Value);

                        // Se aplica la estrategia que valida si pasa o no las reglas de aprobacion
                        //
                        var strategyResult = _antiFraudStrategy.ApplyLimits(message.Value);


                        // Se envia la actualizacion invocando el microservicio de transacciones
                        //
                        var transactionState = new TransactionState()
                        {
                            Id = message.Id,
                            State = strategyResult ? State.Approved : State.Rejected
                        };

                        string json = JsonSerializer.Serialize(transactionState);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");


                        await _clientHttp.PostAsync(_transactionSettings.Url, content);


                        Console.WriteLine($"Mensaje recibido: {consumeResult.Message.Value}");

                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError("Error consumiendo mensaje: {0}", ex.Error.Reason);
                    }
                }
                consumer.Close();
            }

            await Task.CompletedTask;
        }
    }
}
