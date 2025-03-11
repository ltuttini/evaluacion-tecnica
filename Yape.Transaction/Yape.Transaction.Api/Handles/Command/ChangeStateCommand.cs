using MediatR;
using Yape.Transaction.Infrastructure.Entity;

namespace Yape.FinancialTransaction.Handles
{
    public class ChangeStateCommand : IRequest<bool>
    {
        public Guid TransactionId { get; set; }
        public State State { get; set; }
    }
}
