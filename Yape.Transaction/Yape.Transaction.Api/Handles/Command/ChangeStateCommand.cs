using MediatR;
using Yape.Transaction.Infrastructure.Entity;

namespace Yape.FinancialTransaction.Handles
{
    public class ChangeStateCommand : IRequest
    {
        public int Id { get; set; }
        public State State { get; set; }
    }
}
