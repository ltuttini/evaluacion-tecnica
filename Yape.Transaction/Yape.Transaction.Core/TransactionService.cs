using Yape.Transaction.Infrastructure.Data;
using Yape.Transaction.Infrastructure.Entity;
using Yape.Transaction.Infrastructure.Kafka;

namespace Yape.Transaction.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IKafkaProducerService _kafkaProducerService;

        public TransactionService(ITransactionRepository transactionRepository, IKafkaProducerService kafkaProducerService)
        {
            _transactionRepository = transactionRepository;
            _kafkaProducerService = kafkaProducerService;
        }

        public async Task CreateAsync(TransactionEntity transaction)
        {
            await _transactionRepository.AddAsync(transaction);

            await _kafkaProducerService.ProduceAsync(transaction);

        }

        public async Task ChangeStateAsync(TransactionEntity transaction)
        {
            var entity = await _transactionRepository.GetByIdAsync(transaction.Id);

            entity.State = transaction.State;

            await _transactionRepository.UpdateAsync(entity);
        }

    }
}
