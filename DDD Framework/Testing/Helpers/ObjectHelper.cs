using Domain.DTOs;
using Domain.Entities;
using Domain.Resources;
using System;
using System.Data;

namespace Testing.Helpers
{
    public static class ObjectHelper
    {
        public static Guid _id = Guid.NewGuid();
        public static Guid _salt = Guid.NewGuid();
        public static string _mail = "escribanoc.r@outlook.com";
        public static string _password = "ThisIsAPassword";
        public static string _name = "Rodrigo";
        public static string _surname = "Escribano";
        public static string _title = "mr";

        public static Account GetAccount()
        {
            return new Account(_id, _mail, _password, _name, _surname, _title, _salt, DateTime.Now, DateTime.Now.AddMinutes(20));
        }

        public static AccountDto GetAccountDto()
        {
            return new AccountDto(_id, _mail, _name, _surname, _title);
        }

        public static SimpleAccountDto GetSimpleAccountDto()
        {
            return new SimpleAccountDto(_mail, _password, _name, _surname, _title);
        }

        public static UpdateAccountDto GetUpdateAccountDto()
        {
            return new UpdateAccountDto(_mail, _name, _surname, _title);
        }

        public static ArgumentException GetArgumentException()
        {
            return new ArgumentException(String.Format(Resources.NullOrEmptyParameter, "GUID"));
        }

        public static DuplicateNameException GetDuplicateNameException()
        {
            return new DuplicateNameException(String.Format(Resources.AccountAlreadyExists, _mail));
        }
    }
}
