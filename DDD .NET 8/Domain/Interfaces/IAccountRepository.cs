using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts();
        Account GetAccount(Guid UUID);
        Account FindAccountByEmail(string email);
        Account CreateAccount(string email, string password, string name, string surname, string title);
        Account UpdateAccount(Guid UUID, string email, string name, string surname, string title);
        void DeleteAccount(Guid UUID);
    }
}