using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Profiles;
using Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data;
using Testing.Helpers;

namespace Testing.UnitTests.Services
{
    [TestClass]
    public class AccountServiceTests
    {
        private IAccountService _accountService;
        private Mock<IAccountRepository> _accountRepository;
        private Mock<IAuthorizationHelper> _authorizationHelper;
        private static IMapper _autoMapper;

        private Guid _id;
        private AccountDto _accountDto;
        private Account _account;
        private IEnumerable<Account> _accountList;
        private Account _nullAccount;
        private SimpleAccountDto _simpleAccountDto;
        private UpdateAccountDto _updateAccountDto;
        private AuthenticationDto _authenticationDto;
        private string _jwtToken;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AccountProfile());
            });

            _autoMapper = config.CreateMapper();
        }

        [TestInitialize]
        public void Init()
        {
            _accountRepository = new Mock<IAccountRepository>();
            _authorizationHelper = new Mock<IAuthorizationHelper>();
            _accountService = new AccountService(_accountRepository.Object, _authorizationHelper.Object, _autoMapper);

            _id = Guid.NewGuid();
            _accountDto = ObjectHelper.GetAccountDto();
            _account = ObjectHelper.GetAccount();
            _accountList = ObjectHelper.GetAccountList();
            _nullAccount = null;
            _simpleAccountDto = ObjectHelper.GetSimpleAccountDto();
            _updateAccountDto = ObjectHelper.GetUpdateAccountDto();
            _authenticationDto = ObjectHelper.GetAuthenticationDto();
            _jwtToken = ObjectHelper.JwtToken;
        }

        #region GetAllAccounts

        [TestMethod]
        public void GetAllAccounts_Success()
        {
            _accountRepository.Setup(x => x.GetAllAccounts()).Returns(_accountList);
            IEnumerable<AccountDto> result = _accountService.GetAllAccounts();

            Assert.AreEqual(_accountList.FirstOrDefault().Name, result.FirstOrDefault().Name);
            Assert.AreEqual(_accountList.FirstOrDefault().Surname, result.FirstOrDefault().Surname);
        }

        #endregion

        #region GetAccount

        [TestMethod]
        public void GetAccount_Success()
        {
            _accountRepository.Setup(x => x.GetAccount(It.IsAny<Guid>())).Returns(_account);
            AccountDto result = _accountService.GetAccount(_id);

            Assert.AreEqual(_accountDto.Name, result.Name);
            Assert.AreEqual(_accountDto.Surname, result.Surname);
        }

        #endregion

        #region CreateAccount

        [TestMethod]
        public void CreateAccount_Success()
        {
            _accountRepository.Setup(x => x.FindAccountByEmail(It.IsAny<string>())).Returns(_nullAccount);
            _accountRepository.Setup(x => x.CreateAccount(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(_account);
            AccountDto result = _accountService.CreateAccount(_simpleAccountDto);

            Assert.AreEqual(_accountDto.Name, result.Name);
            Assert.AreEqual(_accountDto.Surname, result.Surname);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateNameException))]
        public void CreateAccount_Throws_DuplicateNameExceptionCausedByExistingAccount()
        {
            _accountRepository.Setup(x => x.FindAccountByEmail(It.IsAny<string>())).Returns(_account);
            AccountDto result = _accountService.CreateAccount(_simpleAccountDto);
        }

        #endregion

        #region UpdateAccount

        [TestMethod]
        public void UpdateAccount_Success()
        {
            _accountRepository.Setup(x => x.UpdateAccount(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(_account);
            AccountDto result = _accountService.UpdateAccount(_id, _updateAccountDto);

            Assert.AreEqual(_accountDto.Name, result.Name);
            Assert.AreEqual(_accountDto.Surname, result.Surname);
        }

        #endregion

        #region DeleteAccount

        [TestMethod]
        public void DeleteAccount_Success()
        {
            _accountRepository.Setup(x => x.DeleteAccount(It.IsAny<Guid>()));
            _accountService.DeleteAccount(_id);

            _accountRepository.Verify(x => x.DeleteAccount(It.IsAny<Guid>()), Times.Once());
        }

        #endregion

        #region Authenticate

        [TestMethod]
        public void Authenticate_Success()
        {
            _accountRepository.Setup(x => x.FindAccountByEmail(It.IsAny<string>())).Returns(_account);
            _authorizationHelper.Setup(x => x.ValidateHash(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _authorizationHelper.Setup(x => x.GenerateJwtToken()).Returns(_jwtToken);
            string result = _accountService.Authenticate(_authenticationDto);

            Assert.AreEqual(_jwtToken, result);
        }

        [TestMethod]
        public void Authenticate_IncorrectPassword()
        {
            _accountRepository.Setup(x => x.FindAccountByEmail(It.IsAny<string>())).Returns(_account);
            _authorizationHelper.Setup(x => x.ValidateHash(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            string result = _accountService.Authenticate(_authenticationDto);

            Assert.AreEqual(string.Empty, result);
        }

        #endregion

        #region ValidateUUID

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAccount_Throws_ArgumentExceptionCausedByEmptyGuid()
        {
            AccountDto result = _accountService.GetAccount(Guid.Empty);
        }

        #endregion

        #region ValidateSimpleAccountDto
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateSimpleAccountDto_Throws_ArgumentExceptionCausedByNullEmail()
        {
            _simpleAccountDto.Email = "";
            AccountDto result = _accountService.CreateAccount(_simpleAccountDto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateSimpleAccountDto_Throws_ArgumentExceptionCausedByNullName()
        {
            _simpleAccountDto.Name = "";
            AccountDto result = _accountService.CreateAccount(_simpleAccountDto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateSimpleAccountDto_Throws_ArgumentExceptionCausedByNullSurname()
        {
            _simpleAccountDto.Surname = "";
            AccountDto result = _accountService.CreateAccount(_simpleAccountDto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateSimpleAccountDto_Throws_ArgumentExceptionCausedByNullPassword()
        {
            _simpleAccountDto.Password = "";
            AccountDto result = _accountService.CreateAccount(_simpleAccountDto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateSimpleAccountDto_Throws_ArgumentExceptionCausedByNullTitle()
        {
            _simpleAccountDto.Title = "MoreThan5Characters";
            AccountDto result = _accountService.CreateAccount(_simpleAccountDto);
        }

        #endregion

        #region ValidateUpdateAccountDto

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateUpdateAccountDTO_Throws_ArgumentExceptionCausedByNullEmail()
        {
            _updateAccountDto.Email = "";
            AccountDto result = _accountService.UpdateAccount(_id, _updateAccountDto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateUpdateAccountDTO_Throws_ArgumentExceptionCausedByNullName()
        {
            _updateAccountDto.Name = "";
            AccountDto result = _accountService.UpdateAccount(_id, _updateAccountDto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateUpdateAccountDTO_Throws_ArgumentExceptionCausedByNullSurname()
        {
            _updateAccountDto.Surname = "";
            AccountDto result = _accountService.UpdateAccount(_id, _updateAccountDto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateUpdateAccountDTO_Throws_ArgumentExceptionCausedByNullTitle()
        {
            _updateAccountDto.Title = "MoreThan5Characters";
            AccountDto result = _accountService.UpdateAccount(_id, _updateAccountDto);
        }

        #endregion

        #region ValidateAuthenticationDto

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateAuthenticationDto_Throws_ArgumentExceptionCausedByNullEmail()
        {
            _authenticationDto.Email = "";
            string result = _accountService.Authenticate(_authenticationDto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateAuthenticationDto_Throws_ArgumentExceptionCausedByNullPassword()
        {
            _authenticationDto.Password = "";
            string result = _accountService.Authenticate(_authenticationDto);
        }

        #endregion

        #region ValidateExistingAccount

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateExistingAccount_AccountDoesNotExist()
        {
            _account = null;
            _accountRepository.Setup(x => x.FindAccountByEmail(It.IsAny<string>())).Returns(_account);
            string result = _accountService.Authenticate(_authenticationDto);
        }

        #endregion

    }
}
