using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repository.Models;

namespace Infrastructure.Repository
{
    public class AccountRepository(DDDContext dddContext) : IAccountRepository
    {
        public IEnumerable<Account> GetAllAccounts()
        {
            return dddContext.Accounts;
        }

        public Account GetAccount(Guid uuid)
        {
            return dddContext.Accounts.Find(uuid);
        }

        public Account FindAccountByEmail(string email)
        {
            return dddContext.Accounts.FirstOrDefault(x => x.Email == email);
        }

        //public Response<Account> FindAccountByEmail(string email)
        //{

        //    try
        //    {
        //        return dddContext.Accounts.FirstOrDefault(x => x.Email == email);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Response<Account>.AddError(nameof(Resources.SqlError), HttpStatusCode.InternalServerError, MethodBase.GetCurrentMethod().Name, printError: true, arguments: [ex.ToString()]);
        //    }
        //}

        public Account CreateAccount(string email, string password, string name, string surname, string title)
        {
            var newAccount = new Account(email, password, name, surname, title);

            dddContext.Accounts.Add(newAccount);
            dddContext.SaveChanges();
            return newAccount;
        }

        public Account UpdateAccount(Guid uuid, string email, string name, string surname, string title)
        {
            Account modifiedAccount = dddContext.Accounts.Find(uuid);

            modifiedAccount.Email = email;
            modifiedAccount.Name = name;
            modifiedAccount.Surname = surname;
            modifiedAccount.Title = title;

            dddContext.SaveChanges();
            return modifiedAccount;
        }

        public void DeleteAccount(Guid uuid)
        {
            Account deletedAccount = dddContext.Accounts.Find(uuid);
            dddContext.Accounts.Remove(deletedAccount);
            dddContext.SaveChanges();
        }
    }
}
