using AutoMapper;
using MediatR;
using System.Transactions;
using Yape.Transaction.Infrastructure.Entity;
using Yape.Transaction.Service;

namespace Yape.FinancialTransaction.Handles
{
    public class CrearTransactionHandler : IRequestHandler<CrearTransactionCommand, bool>
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        public CrearTransactionHandler(IMapper mapper, ITransactionService transactionService)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CrearTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = _mapper.Map<TransactionEntity>(request);

            await _transactionService.CreateAsync(transaction);

            return true;
        }
    }
}
