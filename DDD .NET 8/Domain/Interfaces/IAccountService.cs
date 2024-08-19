using Domain.DTOs;

namespace Domain.Interfaces
{
    public interface IAccountService
    {
        AccountDto GetAccount(Guid UUID);
        AccountDto CreateAccount(SimpleAccountDto simpleAccountDto);
        AccountDto UpdateAccount(Guid UUID, UpdateAccountDto updateAccountDto);
        void DeleteAccount(Guid UUID);
        IEnumerable<AccountDto> GetAllAccounts();
        string Authenticate(AuthenticationDto authenticationDto);
    }
}