using AutoMapper;
using MediatR;
using System.Transactions;
using Yape.Transaction.Infrastructure.Entity;
using Yape.Transaction.Service;

namespace Yape.FinancialTransaction.Handles
{
    public class ChangeStateHandler : IRequestHandler<ChangeStateCommand>
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        public ChangeStateHandler(IMapper mapper, ITransactionService transactionService)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        public async Task Handle(ChangeStateCommand request, CancellationToken cancellationToken)
        {
            var transaction = _mapper.Map<TransactionEntity>(request);

            await _transactionService.ChangeStateAsync(transaction);

        }
    }
}
