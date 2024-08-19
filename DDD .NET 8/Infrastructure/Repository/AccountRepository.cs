using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repository.Models;

namespace Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DDDContext _dddContext;

        public AccountRepository(DDDContext dddContext)
        {
            _dddContext = dddContext;
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return _dddContext.Accounts;
        }

        public Account GetAccount(Guid UUID)
        {
            return _dddContext.Accounts.Find(UUID);
        }

        public Account FindAccountByEmail(string email)
        {
            return _dddContext.Accounts.Where(x => x.Email == email).FirstOrDefault();
        }

        public Account CreateAccount(string email, string password, string name, string surname, string title)
        {
            Account newAccount = new Account(email, password, name, surname, title);

            _dddContext.Accounts.Add(newAccount);
            _dddContext.SaveChanges();
            return newAccount;
        }

        public Account UpdateAccount(Guid UUID, string email, string name, string surname, string title)
        {
            Account modifiedAccount = _dddContext.Accounts.Find(UUID);

            modifiedAccount.Email = email;
            modifiedAccount.Name = name;
            modifiedAccount.Surname = surname;
            modifiedAccount.Title = title;

            _dddContext.SaveChanges();
            return modifiedAccount;
        }

        public void DeleteAccount(Guid UUID)
        {
            Account deletedAccount = _dddContext.Accounts.Find(UUID);
            _dddContext.Accounts.Remove(deletedAccount);
            _dddContext.SaveChanges();
        }
    }
}
