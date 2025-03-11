using Yape.Transaction.Infrastructure.Entity;

namespace Yape.Transaction.Infrastructure.Kafka
{
    public interface IKafkaProducerService
    {
        Task ProduceAsync(TransactionEntity mensaje);
    }
}
