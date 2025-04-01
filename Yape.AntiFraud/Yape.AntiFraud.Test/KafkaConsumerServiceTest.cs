using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Text.Json;
using Yape.AntiFraud.Core;
using Yape.AntiFraud.Core.Entity;
using Yape.AntiFraud.Core.Settings;
using Yape.AntiFraud.Strategy;

namespace Yape.AntiFraud.Test
{
    public class KafkaConsumerServiceTest
    {

        private readonly IOptions<KafkaSettings> _kafkaSettings;
        private readonly IOptions<TransactionSettings> _transactionSettings;
        private readonly Mock<ILogger<KafkaConsumerService>> _loggerMock;
        private readonly AntiFraudStrategyFactory _antiFraudStrategy;

        public KafkaConsumerServiceTest()
        {
            _kafkaSettings = Options.Create<KafkaSettings>(new KafkaSettings());
            _transactionSettings = Options.Create<TransactionSettings>(new TransactionSettings() { Url = "http://urlmock" });
            _loggerMock = new Mock<ILogger<KafkaConsumerService>>();
            _antiFraudStrategy = new AntiFraudStrategyFactory();
        }


        [Fact]
        public async Task ExecuteAsync_ProcessTransaction_HttpOk()
        {
            //Init
            var cts = new CancellationTokenSource();

            var message = new TransactionMessage()
            {
                Id = 1,
                Value = 1000
            };

            var consumerMock = new Mock<IConsumer<Ignore, string>>();
            consumerMock.Setup(x => x.Consume(It.IsAny<CancellationToken>()))
                                .Returns(new ConsumeResult<Ignore, string>()
                                {
                                    Message = new Message<Ignore, string>()
                                    {
                                        Value = JsonSerializer.Serialize(message)
                                    }
                                });

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri == new Uri("http://urlmock") &&
                        req.Content != null
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK })
                .Callback<HttpRequestMessage, CancellationToken>(async (request, cancellationToken) =>
                {
                    await cts.CancelAsync();
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            httpClientFactoryMock.Setup(x => x.CreateClient(Options.DefaultName)).Returns(httpClient);

         
            //Act
            var kafkaConsumerService = new KafkaConsumerService(consumerMock.Object, 
                                                                    _kafkaSettings, _transactionSettings,
                                                                    httpClientFactoryMock.Object, 
                                                                    _antiFraudStrategy, 
                                                                    _loggerMock.Object);

           
            await kafkaConsumerService.StartAsync(cts.Token);

            cts.Dispose();

            //Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Http Success")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

        }

        [Fact]
        public async Task ExecuteAsync_ProcessTransaction_HttpFail()
        {
            //Init
            var cts = new CancellationTokenSource();

            var message = new TransactionMessage()
            {
                Id = 1,
                Value = 1000
            };

            var consumerMock = new Mock<IConsumer<Ignore, string>>();
            consumerMock.Setup(x => x.Consume(It.IsAny<CancellationToken>()))
                                .Returns(new ConsumeResult<Ignore, string>()
                                {
                                    Message = new Message<Ignore, string>()
                                    {
                                        Value = JsonSerializer.Serialize(message)
                                    }
                                });

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri == new Uri("http://urlmock") &&
                        req.Content != null
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.BadRequest })
                .Callback<HttpRequestMessage, CancellationToken>(async (request, cancellationToken) =>
                {
                    await cts.CancelAsync();
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            httpClientFactoryMock.Setup(x => x.CreateClient(Options.DefaultName)).Returns(httpClient);

            //Act
            var kafkaConsumerService = new KafkaConsumerService(consumerMock.Object,
                                                                    _kafkaSettings, _transactionSettings,
                                                                    httpClientFactoryMock.Object,
                                                                    _antiFraudStrategy,
                                                                    _loggerMock.Object);


            // Act
            await kafkaConsumerService.StartAsync(cts.Token);

            cts.Dispose();


            //Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Http Error")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

        }

        [Fact]
        public async Task ExecuteAsync_ProcessTransaction_ConsumeException()
        {
            //Init
            var cts = new CancellationTokenSource();

            var consumerMock = new Mock<IConsumer<Ignore, string>>();
            var consumeException = new ConsumeException(new ConsumeResult<byte[], byte[]>(), new Error(ErrorCode.InvalidMsg));
            consumerMock.Setup(x => x.Consume(It.IsAny<CancellationToken>()))
                .Callback(async () =>
                {
                    await cts.CancelAsync();
                })
                .Throws(consumeException);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();

            //Act
            var kafkaConsumerService = new KafkaConsumerService(consumerMock.Object,
                                                                    _kafkaSettings, _transactionSettings,
                                                                    httpClientFactoryMock.Object,
                                                                    _antiFraudStrategy,
                                                                    _loggerMock.Object);


            await kafkaConsumerService.StartAsync(cts.Token);

            cts.Dispose();

            //Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error consumiendo mensaje")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

        }


    }
}