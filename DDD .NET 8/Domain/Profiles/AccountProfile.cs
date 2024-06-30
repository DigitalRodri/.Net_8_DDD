using AutoMapper;
using Domain.DTOs;
using Domain.Entities;

namespace Domain.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile() 
        {
            CreateMap<Account, AccountDto>();
        }
        
    }
}
