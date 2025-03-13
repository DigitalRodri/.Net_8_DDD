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
            Response<IEnumerable<Account>> accountList = _accountRepository.GetAllAccounts();
            if (accountList.HasError)
                return Response<IEnumerable<AccountDto>>.AddError(accountList.Errors);

            var accountListDto = _autoMapper.Map<IEnumerable<AccountDto>>(accountList.Content);
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

            Account existingAccount = _accountRepository.FindAccountByEmail(simpleAccountDto.Email);
            if (existingAccount != null) throw new DuplicateNameException(String.Format(Resources.Resources.AccountAlreadyExists, simpleAccountDto.Email));

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

        public string Authenticate(AuthenticationDto authenticationDto)
        {
            ValidateAuthenticationDto(authenticationDto);
            Account existingAccount = ValidateExistingAccount(authenticationDto);

            if (_authorizationHelper.ValidateHash(authenticationDto.Password, existingAccount.Password))
                return _authorizationHelper.GenerateJwtToken();
            else return string.Empty;
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

        private static void ValidateAuthenticationDto(AuthenticationDto authenticationDto)
        {
            if (string.IsNullOrEmpty(authenticationDto.Email))
                throw new ArgumentException(String.Format(Resources.Resources.NullOrEmptyParameter, nameof(authenticationDto.Email)));
            if (string.IsNullOrEmpty(authenticationDto.Password))
                throw new ArgumentException(String.Format(Resources.Resources.NullOrEmptyParameter, nameof(authenticationDto.Password)));
        }

        private Account ValidateExistingAccount(AuthenticationDto authenticationDto)
        {
            Account existingAccount = _accountRepository.FindAccountByEmail(authenticationDto.Email);
            if (existingAccount == null)
                throw new ArgumentException(String.Format(Resources.Resources.AccountDoesNotExist, authenticationDto.Email));
            return existingAccount;
        }

        #endregion
    }
}
