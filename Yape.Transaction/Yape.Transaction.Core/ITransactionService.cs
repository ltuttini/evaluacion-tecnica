using Yape.Transaction.Infrastructure.Entity;

namespace Yape.Transaction.Service
{
    public interface ITransactionService
    {
        Task CreateAsync(TransactionEntity transaction);
    }
}