using Domain.Entities;
using Domain.Helpers;

namespace Domain.Interfaces
{
    public interface IAccountRepository
    {
        Response<IEnumerable<Account>> GetAllAccounts();
        Response<Account> GetAccount(Guid uuid);
        Account FindAccountByEmail(string email);
        Account CreateAccount(string email, string password, string name, string surname, string title);
        Account UpdateAccount(Guid uuid, string email, string name, string surname, string title);
        void DeleteAccount(Guid uuid);
    }
}