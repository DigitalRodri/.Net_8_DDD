using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces;
using System.Data;
using System.Net;

namespace Domain.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAuthorizationHelper _authorizationHelper;
        private readonly IMapper _autoMapper;

        public AccountService(IAccountRepository accountRepository, IAuthorizationHelper authorizationHelper, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _authorizationHelper = authorizationHelper;
            _autoMapper = mapper;
        }

        public Response<IEnumerable<AccountDto>> GetAllAccounts()
        {
            IEnumerable<Account> accountList = _accountRepository.GetAllAccounts();

            var accountListDto = _autoMapper.Map<IEnumerable<AccountDto>>(accountList);
            return Response<IEnumerable<AccountDto>>.AddContent(accountListDto);
        }

        public Response<AccountDto> GetAccount(Guid UUID)
        {
            Response<bool> uuidValidation = ValidateUuid(UUID);
            if (uuidValidation.HasError)
                return Response<AccountDto>.AddError(uuidValidation.Errors);

            Response<Account> account = _accountRepository.GetAccount(UUID);
            if (account.HasError)
                return Response<AccountDto>.AddError(account.Errors);

            return _autoMapper.Map<AccountDto>(account.Content);
        }

        public AccountDto CreateAccount(SimpleAccountDto simpleAccountDto)
        {
            ValidateSimpleAccountDto(simpleAccountDto);

            Response<Account> existingAccount = _accountRepository.FindAccountByEmail(simpleAccountDto.Email);
            if (existingAccount.Content != null) throw new DuplicateNameException(String.Format(Resources.Resources.AccountAlreadyExists, simpleAccountDto.Email));

            string hashedPassword = _authorizationHelper.Hash(simpleAccountDto.Password);

            Account account = _accountRepository
                .CreateAccount(simpleAccountDto.Email, hashedPassword, simpleAccountDto.Name, simpleAccountDto.Surname, simpleAccountDto.Title);

            return _autoMapper.Map<AccountDto>(account);
        }

        public AccountDto UpdateAccount(Guid UUID, UpdateAccountDto updateAccountDto)
        {
            ValidateUUID(UUID);
            ValidateUpdateAccountDto(updateAccountDto);

            Account modifiedAccount = _accountRepository
                .UpdateAccount(UUID, updateAccountDto.Email, updateAccountDto.Name, updateAccountDto.Surname, updateAccountDto.Title);

            return _autoMapper.Map<AccountDto>(modifiedAccount);
        }

        public void DeleteAccount(Guid UUID)
        {
            ValidateUUID(UUID);

            _accountRepository.DeleteAccount(UUID);

            return;
        }

        public Response<string> Authenticate(AuthenticationDto authenticationDto)
        {
            Response<bool> validation = ValidateAuthenticationDto(authenticationDto);
            if (validation.HasError)
                return Response<string>.AddError(validation.Errors);

            Response<Account> existingAccount = ValidateExistingAccount(authenticationDto);
            if (existingAccount.HasError)
                return Response<string>.AddError(existingAccount.Errors);

            if (_authorizationHelper.ValidateHash(authenticationDto.Password, existingAccount.Content.Password))
                return _authorizationHelper.GenerateJwtToken();

            return Response<string>.AddError(nameof(Resources.Resources.IncorrectPassword), HttpStatusCode.Unauthorized);
        }

        #region Private methods

        private static Response<bool> ValidateUuid(Guid uuid)
        {
            return uuid == Guid.Empty ? Response<bool>.AddError(nameof(Resources.Resources.NullParameter), HttpStatusCode.BadRequest, arguments: ["uuid"]) : Response<bool>.AddContent(true);
        }

        private static void ValidateUUID(Guid UUID)
        {
            if (UUID == Guid.Empty)
                throw new ArgumentException(String.Format(Resources.Resources.NullParameter, nameof(UUID)));
        }

        private static void ValidateSimpleAccountDto(SimpleAccountDto simpleAccountDto)
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

        private static void ValidateUpdateAccountDto(UpdateAccountDto updateAccountDto)
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
                return Response<bool>.AddError(nameof(Resources.Resources.NullOrEmptyParameter), HttpStatusCode.BadRequest, arguments: ["Email"]);
            if (string.IsNullOrEmpty(authenticationDto.Password))
                return Response<bool>.AddError(nameof(Resources.Resources.NullOrEmptyParameter), HttpStatusCode.BadRequest, arguments: ["Password"]);
            return Response<bool>.AddContent(true);
        }

        private Response<Account> ValidateExistingAccount(AuthenticationDto authenticationDto)
        {
            Account existingAccount = _accountRepository.FindAccountByEmail(authenticationDto.Email);

            return existingAccount ?? Response<Account>.AddError(nameof(Resources.Resources.AccountDoesNotExist), HttpStatusCode.BadRequest, arguments: ["Email"]);
        }

        #endregion
    }
}
