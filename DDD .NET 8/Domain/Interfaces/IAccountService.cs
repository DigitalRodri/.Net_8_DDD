using Domain.DTOs;
using Domain.Helpers;

namespace Domain.Interfaces
{
    public interface IAccountService
    {
        Response<AccountDto> GetAccount(Guid UUID);
        AccountDto CreateAccount(SimpleAccountDto simpleAccountDto);
        AccountDto UpdateAccount(Guid UUID, UpdateAccountDto updateAccountDto);
        void DeleteAccount(Guid UUID);
        Response<IEnumerable<AccountDto>> GetAllAccounts();
        Response<string> Authenticate(AuthenticationDto authenticationDto);
    }
}