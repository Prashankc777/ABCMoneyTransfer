using ABCMoneyTransfer.Data.DTOs;
using ABCMoneyTransfer.Data.Entities;
using AutoMapper;

namespace ABCMoneyTransfer.App.Profiles
{
    public class TransactionMapper : Profile
    {
        public TransactionMapper()
        {
            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<TransactionInsertDto, Transaction>();
        }
    }
}
