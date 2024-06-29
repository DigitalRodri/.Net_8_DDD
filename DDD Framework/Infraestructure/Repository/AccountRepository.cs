using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Repository.Models;
using System;
using System.Linq;

namespace Infraestructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        public Account GetAccount(Guid UUID)
        {
            using (var db = new DDDContext())
            {
                return db.Accounts.Find(UUID);
            }
        }

        public Account FindAccountByEmail(string email)
        {
            using (var db = new DDDContext())
            {
                return db.Accounts.Where(x => x.Email == email).FirstOrDefault();
            }
        }

        public Account CreateAccount(string email, string password, string name, string surname, string title, Guid salt)
        {
            Account newAccount = new Account(email, password, name, surname, title, salt);

            using (var db = new DDDContext())
            {
                Account result = db.Accounts.Add(newAccount);
                db.SaveChanges();
                return result;
            }
        }

        public Account UpdateAccount(Guid UUID, string email, string name, string surname, string title)
        {
            using (var db = new DDDContext())
            {
                Account modifiedAccount = db.Accounts.Find(UUID);

                modifiedAccount.Email = email;
                modifiedAccount.Name = name;
                modifiedAccount.Surname = surname;
                modifiedAccount.Title = title;

                db.SaveChanges();
                return modifiedAccount;
            }
        }

        public void DeleteAccount(Guid UUID)
        {
            using (var db = new DDDContext())
            {
                Account deletedAccount = db.Accounts.Find(UUID);
                db.Accounts.Remove(deletedAccount);
                db.SaveChanges();
            }
        }
    }
}
