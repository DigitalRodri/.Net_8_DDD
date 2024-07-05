using Domain.DTOs;
using System;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IAccountService
    {
        AccountDto GetAccount(Guid UUID);
        AccountDto CreateAccount(SimpleAccountDto simpleAccountDto);
        AccountDto UpdateAccount(Guid UUID, UpdateAccountDto updateAccountDto);
        void DeleteAccount(Guid UUID);
        IEnumerable<AccountDto> GetAllAccounts();
    }
}