using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces;
using System.Data;

namespace Domain.Services
{
    public class AccountService(
        IAccountRepository accountRepository,
        IAuthorizationHelper authorizationHelper,
        IMapper mapper)
        : IAccountService
    {
        public Response<IEnumerable<AccountDto>> GetAllAccounts()
        {
            IEnumerable<Account> accountList = accountRepository.GetAllAccounts();

            var accountListDto = mapper.Map<IEnumerable<AccountDto>>(accountList);
            return Response<IEnumerable<AccountDto>>.AddContent(accountListDto);
        }

        public Response<AccountDto> GetAccount(Guid uuid)
        {
            Response<bool> uuidValidation = ValidateUuid(uuid);
            if (uuidValidation.HasError)
                return Response<AccountDto>.AddError(uuidValidation.Error);

            Account account = accountRepository.GetAccount(uuid);

            return account == null ? Response<AccountDto>.AddError(nameof(Resources.Resources.AccountDoesNotExist), arguments: [uuid.ToString()]) : mapper.Map<AccountDto>(account);
        }

        public AccountDto CreateAccount(SimpleAccountDto simpleAccountDto)
        {
            ValidateSimpleAccountDtoOrFail(simpleAccountDto);

            Response<Account> existingAccount = accountRepository.FindAccountByEmail(simpleAccountDto.Email);
            if (existingAccount.Content != null) throw new DuplicateNameException(String.Format(Resources.Resources.AccountAlreadyExists, simpleAccountDto.Email));

            string hashedPassword = authorizationHelper.Hash(simpleAccountDto.Password);

            Account account = accountRepository
                .CreateAccount(simpleAccountDto.Email, hashedPassword, simpleAccountDto.Name, simpleAccountDto.Surname, simpleAccountDto.Title);

            return mapper.Map<AccountDto>(account);
        }

        public AccountDto UpdateAccount(Guid UUID, UpdateAccountDto updateAccountDto)
        {
            ValidateUUID(UUID);
            ValidateUpdateAccountDtoOrFail(updateAccountDto);

            Account modifiedAccount = accountRepository
                .UpdateAccount(UUID, updateAccountDto.Email, updateAccountDto.Name, updateAccountDto.Surname, updateAccountDto.Title);

            return mapper.Map<AccountDto>(modifiedAccount);
        }

        public void DeleteAccount(Guid UUID)
        {
            ValidateUUID(UUID);

            accountRepository.DeleteAccount(UUID);

            return;
        }

        public Response<string> Authenticate(AuthenticationDto authenticationDto)
        {
            Response<bool> validation = ValidateAuthenticationDto(authenticationDto);
            if (validation.HasError)
                return Response<string>.AddError(validation.Error);

            Response<Account> existingAccount = ValidateExistingAccount(authenticationDto);
            if (existingAccount.HasError)
                return Response<string>.AddError(existingAccount.Error);

            if (authorizationHelper.ValidateHash(authenticationDto.Password, existingAccount.Content.Password))
                return authorizationHelper.GenerateJwtToken();

            return Response<string>.AddError(nameof(Resources.Resources.IncorrectPassword), printError: true);
        }

        #region Private methods

        private static Response<bool> ValidateUuid(Guid uuid)
        {
            return uuid == Guid.Empty ? Response<bool>.AddError(nameof(Resources.Resources.NullParameter), arguments: ["uuid"]) : Response<bool>.AddContent(true);
        }

        private static void ValidateUUID(Guid UUID)
        {
            if (UUID == Guid.Empty)
                throw new ArgumentException(String.Format(Resources.Resources.NullParameter, nameof(UUID)));
        }

        private static void ValidateSimpleAccountDtoOrFail(SimpleAccountDto simpleAccountDto)
        {
            if (string.IsNullOrEmpty(simpleAccountDto.Email))
                throw new ArgumentException(String.Format(Resources.Resources.NullOrEmptyParameter, nameof(simpleAccountDto.Email)));
            if (string.IsNullOrEmpty(simpleAccountDto.Name))
                throw new ArgumentException(String.Format(Resources.Resources.NullOrEmptyParameter, nameof(simpleAccountDto.Name)));
            if (string.IsNullOrEmpty(simpleAccountDto.Surname))
                throw new ArgumentException(String.Format(Resources.Resources.NullOrEmptyParameter, nameof(simpleAccountDto.Surname)));
            if (string.IsNullOrEmpty(simpleAccountDto.Password))
                throw new ArgumentException(String.Format(Resources.Resources.NullOrEmptyParameter, nameof(simpleAccountDto.Password)));
            if (!string.IsNullOrEmpty(simpleAccountDto.Title) && simpleAccountDto.Title.Length > 5)
                throw new ArgumentException(String.Format(Resources.Resources.TitleLengthError, simpleAccountDto.Title));
        }

        private static void ValidateUpdateAccountDtoOrFail(UpdateAccountDto updateAccountDto)
        {
            if (string.IsNullOrEmpty(updateAccountDto.Email))
                throw new ArgumentException(String.Format(Resources.Resources.NullOrEmptyParameter, nameof(updateAccountDto.Email)));
            if (string.IsNullOrEmpty(updateAccountDto.Name))
                throw new ArgumentException(String.Format(Resources.Resources.NullOrEmptyParameter, nameof(updateAccountDto.Name)));
            if (string.IsNullOrEmpty(updateAccountDto.Surname))
                throw new ArgumentException(String.Format(Resources.Resources.NullOrEmptyParameter, nameof(updateAccountDto.Surname)));
            if (!string.IsNullOrEmpty(updateAccountDto.Title) && updateAccountDto.Title.Length > 5)
                throw new ArgumentException(String.Format(Resources.Resources.TitleLengthError, updateAccountDto.Title));
        }

        private static Response<bool> ValidateAuthenticationDto(AuthenticationDto authenticationDto)
        {
            if (string.IsNullOrEmpty(authenticationDto.Email))
                return Response<bool>.AddError(nameof(Resources.Resources.NullOrEmptyParameter), arguments: ["Email"]);
            if (string.IsNullOrEmpty(authenticationDto.Password))
                return Response<bool>.AddError(nameof(Resources.Resources.NullOrEmptyParameter), arguments: ["Password"]);
            return Response<bool>.AddContent(true);
        }

        private Response<Account> ValidateExistingAccount(AuthenticationDto authenticationDto)
        {
            Account existingAccount = accountRepository.FindAccountByEmail(authenticationDto.Email);

            return existingAccount ?? Response<Account>.AddError(nameof(Resources.Resources.AccountDoesNotExist), arguments: ["Email"]);
        }

        #endregion
    }
}
