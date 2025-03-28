using Yape.Transaction.Infrastructure.Entity;

namespace Yape.Transaction.Infrastructure.Data
{
    public interface ITransactionRepository : IRepository<TransactionEntity>
    {

    }

    public class TransactionRepository : Repository<TransactionEntity>, ITransactionRepository
    {
        public TransactionRepository(TransactionDbContext context) : base(context)
        {
        }

    }
}
