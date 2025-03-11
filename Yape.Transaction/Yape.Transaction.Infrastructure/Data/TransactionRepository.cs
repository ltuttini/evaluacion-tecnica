using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Yape.Transaction.Infrastructure.Entity;

namespace Yape.Transaction.Infrastructure.Data
{
    public interface ITransactionRepository : IRepository<TransactionEntity>
    {

    }

    public class TransactionRepository : Repository<TransactionEntity>, ITransactionRepository
    {
        private readonly TransactionDbContext _context;

        public TransactionRepository(TransactionDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
