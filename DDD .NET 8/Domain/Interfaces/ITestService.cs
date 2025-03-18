using Domain.DTOs;
using Domain.Helpers;

namespace Domain.Interfaces
{
    public interface ITestService
    {
        Response<AccountDto> GetAccountResult(Guid uuid);
        AccountDto GetAccountException(Guid uuid);
    }
}