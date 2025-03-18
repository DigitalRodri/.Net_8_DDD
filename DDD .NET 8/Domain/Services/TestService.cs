using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces;

namespace Domain.Services
{
    public class TestService(
        IAccountRepository accountRepository,
        IAuthorizationHelper authorizationHelper,
        IMapper mapper)
        : ITestService
    {
        public Response<AccountDto> GetAccountResult(Guid uuid)
        {
            Response<bool> uuidValidation = ValidateUuid(uuid);
            if (uuidValidation.HasError)
                return Response<AccountDto>.AddError(uuidValidation.Error);

            Account account = accountRepository.GetAccount(uuid);

            return account == null ? Response<AccountDto>.AddError(nameof(Resources.Resources.AccountDoesNotExist), arguments: [uuid.ToString()]) : mapper.Map<AccountDto>(account);
        }

        private static Response<bool> ValidateUuid(Guid uuid)
        {
            return uuid == Guid.Empty ? Response<bool>.AddError(nameof(Resources.Resources.NullParameter), arguments: ["uuid"]) : Response<bool>.AddContent(true);
        }

        public AccountDto GetAccountException(Guid uuid)
        {
            {
                ValidateUuidOrFail(uuid);

                Account account = accountRepository.GetAccount(uuid);

                return account != null ? mapper.Map<AccountDto>(account) : throw new KeyNotFoundException(string.Format(Resources.Resources.NullOrEmptyParameter, "uuid"));
            }
        }

        private static void ValidateUuidOrFail(Guid uuid)
        {
            if (uuid == Guid.Empty)
                throw new ArgumentException(string.Format(Resources.Resources.NullOrEmptyParameter, "uuid"));
        }
    }
}
