using Domain.DTOs;
using System;

namespace Domain.Interfaces
{
    public interface IAccountService
    {
        AccountDto GetAccount(Guid UUID);
        AccountDto CreateAccount(SimpleAccountDto simpleAccountDto);
        AccountDto UpdateAccount(Guid UUID, UpdateAccountDto updateAccountDto);
        void DeleteAccount(Guid UUID);
    }
}