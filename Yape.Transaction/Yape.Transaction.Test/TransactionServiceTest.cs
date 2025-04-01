using Confluent.Kafka;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Yape.Transaction.Infrastructure.Data;
using Yape.Transaction.Infrastructure.Entity;
using Yape.Transaction.Infrastructure.Kafka;
using Yape.Transaction.Service;

namespace Yape.Transaction.Test
{
    public class TransactionServiceTest
    {

        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly IOptions<KafkaSettings> _kafkaSettings;
        private readonly Mock<ILogger<KafkaProducerService>> _loggerMock;

        public TransactionServiceTest()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _kafkaSettings = Options.Create<KafkaSettings>(new KafkaSettings());
            _loggerMock = new Mock<ILogger<KafkaProducerService>>();
        }

        [Fact]
        public async Task Create_CreateTransaction_Ok()
        {
            //Init
            var producerMock = new Mock<IProducer<Null, string>>();
            producerMock.Setup(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<Null, string>>(), It.IsAny<CancellationToken>()))
                                .Returns(Task.FromResult(new DeliveryResult<Null, string>()));

            //Act
            var kafkaProducerService = new KafkaProducerService(producerMock.Object, _kafkaSettings, _loggerMock.Object);

            var transactionService = new TransactionService(_transactionRepositoryMock.Object, kafkaProducerService);

            await transactionService.CreateAsync(new TransactionEntity());


            //Assert
            producerMock.Verify(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<Null, string>>(), It.IsAny<CancellationToken>()), Times.Once);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Mensaje entregado")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Create_CreateTransaction_KafkaFail_ProduceException()
        {
            
            //Init
            var producerMock = new Mock<IProducer<Null, string>>();
            var expectedException = new ProduceException<Null, string>(new Error(ErrorCode.Local_Fail, ""), null);
            producerMock.Setup(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<Null, string>>(), It.IsAny<CancellationToken>())).Throws(expectedException);
            
            //Act
            var kafkaProducerService = new KafkaProducerService(producerMock.Object, _kafkaSettings, _loggerMock.Object);

            var transactionService = new TransactionService(_transactionRepositoryMock.Object, kafkaProducerService);


            //Assert
            await Assert.ThrowsAsync<ProduceException<Null, string>>(
                            async () => await transactionService.CreateAsync(new TransactionEntity()));


            _loggerMock.Verify(
                    x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Falló la entrega del mensaje")),
                        null,
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);

        }

    }
}