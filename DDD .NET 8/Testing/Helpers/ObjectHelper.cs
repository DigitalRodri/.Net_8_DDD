using Domain.DTOs;
using Domain.Entities;
using Domain.Helpers;
using Domain.Resources;
using System.Data;

namespace Testing.Helpers
{
    public static class ObjectHelper
    {
        public static readonly Guid _id = Guid.NewGuid();
        public const string Mail = "escribanoc.r@outlook.com";
        public const string Password = "ThisIsAPassword";
        public const string Name = "Rodrigo";
        public const string Surname = "Escribano";
        public const string Title = "mr";
        public const string UpdatedName = "Ada";
        public const string UpdatedSurname = "Lovelace";
        public const string UpdatedTitle = "mrs";
        public const string JwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE3MjAzNjUwNjAsImV4cCI6MTcyMDM2Njg2MCwiaWF0IjoxNzIwMzY1MDYwLCJpc3MiOiJSb2RyaWdvIEVzY3JpYmFubyJ9.Z4FSZWMPNjHe42lBpZdOGldVE40tF_Tn-sY1OPD_7rg";

        public static Response<T> GetResponse<T>(T content)
        {
            return new Response<T>(content);
        }

        public static Account GetAccount()
        {
            return new Account(_id, Mail, Password, Name, Surname, Title, DateTime.Now, DateTime.Now.AddMinutes(20));
        }

        public static IEnumerable<Account> GetAccountList()
        {
            return new List<Account>() { GetAccount(), GetAccount() };
        }

        public static AccountDto GetAccountDto()
        {
            return new AccountDto(_id, Mail, Name, Surname, Title);
        }

        public static IEnumerable<AccountDto> GetAccountDtoList()
        {
            return new List<AccountDto>() { GetAccountDto(), GetAccountDto() };
        }

        public static SimpleAccountDto GetSimpleAccountDto()
        {
            return new SimpleAccountDto(Mail, Password, Name, Surname, Title);
        }

        public static UpdateAccountDto GetUpdateAccountDto()
        {
            return new UpdateAccountDto(Mail, UpdatedName, UpdatedSurname, UpdatedTitle);
        }

        public static AuthenticationDto GetAuthenticationDto()
        {
            return new AuthenticationDto(Mail, Surname);
        }

        public static ArgumentException GetArgumentException()
        {
            return new ArgumentException(String.Format(Resources.NullOrEmptyParameter, "GUID"));
        }

        public static DuplicateNameException GetDuplicateNameException()
        {
            return new DuplicateNameException(String.Format(Resources.AccountAlreadyExists, Mail));
        }
    }
}
