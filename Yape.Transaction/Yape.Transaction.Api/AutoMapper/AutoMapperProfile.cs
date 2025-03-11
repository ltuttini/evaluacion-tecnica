using AutoMapper;
using Yape.FinancialTransaction.Handles;
using Yape.Transaction.Infrastructure.Entity;

namespace Yape.Transaction.Api.automapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CrearTransactionCommand, TransactionEntity>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => State.Pending));

            CreateMap<ChangeStateCommand, TransactionEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TransactionId));

        }

    }
}
