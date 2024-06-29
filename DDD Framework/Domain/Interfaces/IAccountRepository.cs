using Domain.Entities;
using System;

namespace Domain.Interfaces
{
    public interface IAccountRepository
    {
        Account GetAccount(Guid UUID);
        Account FindAccountByEmail(string email);
        Account CreateAccount(string email, string password, string name, string surname, string title, Guid salt);
        Account UpdateAccount(Guid UUID, string email, string name, string surname, string title);
        void DeleteAccount(Guid UUID);
    }
}