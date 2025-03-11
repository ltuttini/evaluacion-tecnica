using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yape.Transaction.Infrastructure.Entity
{
    public class TransactionEntity
    {
        public int Id { get; set; }
        public Guid SourceAccountId { get; set; }
        public Guid TargetAccountId { get; set; }
        public int TransferTypeId { get; set; }

        public State State { get; set; }
        public int Value { get; set; }
    }

    public enum State
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }

}
