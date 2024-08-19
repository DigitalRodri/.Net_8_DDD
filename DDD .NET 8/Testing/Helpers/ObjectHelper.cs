using Domain.DTOs;
using Domain.Entities;
using Domain.Resources;
using System.Data;

namespace Testing.Helpers
{
    public static class ObjectHelper
    {
        public static Guid _id = Guid.NewGuid();
        public static string _mail = "escribanoc.r@outlook.com";
        public static string _password = "ThisIsAPassword";
        public static string _name = "Rodrigo";
        public static string _surname = "Escribano";
        public static string _title = "mr";
        public static string _jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE3MjAzNjUwNjAsImV4cCI6MTcyMDM2Njg2MCwiaWF0IjoxNzIwMzY1MDYwLCJpc3MiOiJSb2RyaWdvIEVzY3JpYmFubyJ9.Z4FSZWMPNjHe42lBpZdOGldVE40tF_Tn-sY1OPD_7rg";

        public static Account GetAccount()
        {
            return new Account(_id, _mail, _password, _name, _surname, _title, DateTime.Now, DateTime.Now.AddMinutes(20));
        }

        public static IEnumerable<Account> GetAccountList()
        {
            return new List<Account>() { GetAccount(), GetAccount() };
        }

        public static AccountDto GetAccountDto()
        {
            return new AccountDto(_id, _mail, _name, _surname, _title);
        }

        public static IEnumerable<AccountDto> GetAccountDtoList()
        {
            return new List<AccountDto>() { GetAccountDto(), GetAccountDto() };
        }

        public static SimpleAccountDto GetSimpleAccountDto()
        {
            return new SimpleAccountDto(_mail, _password, _name, _surname, _title);
        }

        public static UpdateAccountDto GetUpdateAccountDto()
        {
            return new UpdateAccountDto(_mail, _name, _surname, _title);
        }

        public static AuthenticationDto GetAuthenticationDto()
        {
            return new AuthenticationDto(_mail, _surname);
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
