using AutoMapper;
using BankBuddy.Application.DTOs.Transaction;
using BankBuddy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.Mappings
{
    public class TransactionMappingProfile : Profile
    {
        public TransactionMappingProfile()
        {
            CreateMap<BankAccount, AccountSummary>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.BankAccountId))
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
                .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src =>
                    string.Join(" ", new[]
                    {
                        src.User.FirstName,
                        src.User.MiddleName,
                        src.User.LastName
                    }.Where(n => !string.IsNullOrWhiteSpace(n)))))
                .ForMember(dest => dest.PreviousBalance, opt => opt.Ignore())
                .ForMember(dest => dest.NewBalance, opt => opt.Ignore());

            CreateMap<Transaction, TransactionReceipt>()
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ReferenceId, opt => opt.MapFrom(src => src.ReferenceId ?? string.Empty));

            CreateMap<Transaction, TransferReceiptDTO>()
                .IncludeBase<Transaction, TransactionReceipt>()
                .ForMember(dest => dest.FromAccount, opt => opt.Ignore())
                .ForMember(dest => dest.ToAccount, opt => opt.Ignore());

            CreateMap<Transaction, DepositReceiptDTO>()
                .IncludeBase<Transaction, TransactionReceipt>()
                .ForMember(dest => dest.ToAccount, opt => opt.Ignore());

            CreateMap<Transaction, WithdrawReceiptDTO>()
                .IncludeBase<Transaction, TransactionReceipt>()
                .ForMember(dest => dest.FromAccount, opt => opt.Ignore());

            CreateMap<Transaction, TransactionHistoryDTO>()
                .ForMember(dest => dest.FromAccountNumber, opt => opt.MapFrom(src => src.FromAccount != null ? src.FromAccount.AccountNumber : null))
                .ForMember(dest => dest.FromAccountName, opt => opt.MapFrom(src => src.FromAccount != null
                    ? string.Join(" ", new[]
                    {
                        src.FromAccount.User.FirstName,
                        src.FromAccount.User.MiddleName,
                        src.FromAccount.User.LastName
                    }.Where(n => !string.IsNullOrWhiteSpace(n)))
                    : null))
                .ForMember(dest => dest.ToAccountNumber, opt => opt.MapFrom(src => src.ToAccount != null ? src.ToAccount.AccountNumber : null))
                .ForMember(dest => dest.ToAccountName, opt => opt.MapFrom(src => src.ToAccount != null
                    ? string.Join(" ", new[]
                    {
                        src.ToAccount.User.FirstName,
                        src.ToAccount.User.MiddleName,
                        src.ToAccount.User.LastName
                    }.Where(n => !string.IsNullOrWhiteSpace(n)))
                    : null));

        }
    }
}
