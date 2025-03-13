using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces;
using Domain.Resources;
using Infrastructure.Repository.Models;
using System.Net;
using System.Reflection;

namespace Infrastructure.Repository
{
    public class AccountRepository(DDDContext dddContext) : IAccountRepository
    {
        public Response<IEnumerable<Account>> GetAllAccounts()
        {
            try
            {
                return dddContext.Accounts;
            }
            catch (Exception ex)
            {
                return Response<IEnumerable<Account>>.AddError(nameof(Resources.SqlError), HttpStatusCode.InternalServerError, MethodBase.GetCurrentMethod().Name, printError: true, arguments: [ex.ToString()]);
            }
        }

        public Response<Account> GetAccount(Guid uuid)
        {
            try
            {
                return dddContext.Accounts.Find(uuid);
            }
            catch (Exception ex)
            {
                return Response<Account>.AddError(nameof(Resources.SqlError), HttpStatusCode.InternalServerError, MethodBase.GetCurrentMethod().Name, printError: true, arguments: [ex.ToString()]);
            }
        }

        public Account FindAccountByEmail(string email)
        {
            return dddContext.Accounts.Where(x => x.Email == email).FirstOrDefault();
        }

        public Account CreateAccount(string email, string password, string name, string surname, string title)
        {
            Account newAccount = new Account(email, password, name, surname, title);

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
