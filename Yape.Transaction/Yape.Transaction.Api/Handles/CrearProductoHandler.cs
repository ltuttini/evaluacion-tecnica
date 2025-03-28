using AutoMapper;
using MediatR;
using System.Transactions;
using Yape.Transaction.Infrastructure.Entity;
using Yape.Transaction.Service;

namespace Yape.FinancialTransaction.Handles
{
    public class CrearTransactionHandler : IRequestHandler<CrearTransactionCommand>
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        public CrearTransactionHandler(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        public async Task Handle(CrearTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = _mapper.Map<TransactionEntity>(request);
            
            await _transactionService.CreateAsync(transaction);

        }
    }
}
