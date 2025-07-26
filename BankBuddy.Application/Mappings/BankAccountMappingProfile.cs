using AutoMapper;
using BankBuddy.Application.DTOs.BankAccount;
using BankBuddy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.Mappings
{
    public class BankAccountMappingProfile : Profile
    {
        public BankAccountMappingProfile()
        {
            CreateMap<BankAccount, BankAccountResponseDTO>();
        }
    }
}
