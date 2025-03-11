using MediatR;

namespace Yape.FinancialTransaction.Handles
{
    public class CrearTransactionCommand : IRequest<bool>
    {
        public Guid SourceAccountId { get; set; }
        public Guid TargetAccountId { get; set; }
        public int TransferTypeId { get; set; }
        public int Value { get; set; }
    }
}
