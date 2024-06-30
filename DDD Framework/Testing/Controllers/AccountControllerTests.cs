using Application.Controllers;
using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data;
using System.Net;
using System.Net.Http;
using Testing.Helpers;

namespace Testing.Services
{
    [TestClass]
    public class AccountControllerTests
    {
        private AccountController _accountController;
        private Mock<IAccountService> _accountService;

        private Guid _id;
        private AccountDto _accountDto;
        private SimpleAccountDto _simpleAccountDto;
        private UpdateAccountDto _updateAccountDto;
        private ArgumentException _argumentException;
        private DuplicateNameException _duplicateNameException;

        [TestInitialize]
        public void Init() 
        {


            _accountService = new Mock<IAccountService>();
            _accountController = new AccountController(_accountService.Object);
            _accountController.Request = new HttpRequestMessage();
            _accountController.Request.SetConfiguration(new HttpConfiguration());

            _id = Guid.NewGuid();
            _accountDto = ObjectHelper.GetAccountDto();
            _simpleAccountDto = ObjectHelper.GetSimpleAccountDto();
            _updateAccountDto = ObjectHelper.GetUpdateAccountDto();
            _argumentException = ObjectHelper.GetArgumentException();
            _duplicateNameException = ObjectHelper.GetDuplicateNameException();
        }

        #region GetAccount

        [TestMethod]
        public void GetAccount_Success()
        {
            _accountService.Setup(x => x.GetAccount(It.IsAny<Guid>())).Returns(_accountDto);
            HttpResponseMessage result = _accountController.GetAccount(_id);

            result.TryGetContentValue<AccountDto>(out AccountDto accountResult);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(_accountDto.UUID, accountResult.UUID);
        }

        [TestMethod]
        public void GetAccount_ArgumentException()
        {
            _accountService.Setup(x => x.GetAccount(It.IsAny<Guid>())).Throws(_argumentException);
            HttpResponseMessage result = _accountController.GetAccount(_id);

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void GetAccount_Exception()
        {
            _accountService.Setup(x => x.GetAccount(It.IsAny<Guid>())).Throws(new Exception());
            HttpResponseMessage result = _accountController.GetAccount(_id);

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }

        #endregion

        #region CreateAccount

        [TestMethod]
        public void CreateAccount_Success()
        {
            _accountService.Setup(x => x.CreateAccount(It.IsAny<SimpleAccountDto>())).Returns(_accountDto);
            HttpResponseMessage result = _accountController.CreateAccount(_simpleAccountDto);

            result.TryGetContentValue<AccountDto>(out AccountDto accountResult);
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            Assert.AreEqual(_accountDto.UUID, accountResult.UUID);
        }

        [TestMethod]
        public void CreateAccount_ArgumentException()
        {
            _accountService.Setup(x => x.CreateAccount(It.IsAny<SimpleAccountDto>())).Throws(_argumentException);
            HttpResponseMessage result = _accountController.CreateAccount(_simpleAccountDto);

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void CreateAccount_DuplicateNameException()
        {
            _accountService.Setup(x => x.CreateAccount(It.IsAny<SimpleAccountDto>())).Throws(_duplicateNameException);
            HttpResponseMessage result = _accountController.CreateAccount(_simpleAccountDto);

            Assert.AreEqual(HttpStatusCode.Conflict, result.StatusCode);
        }

        [TestMethod]
        public void CreateAccount_Exception()
        {
            _accountService.Setup(x => x.CreateAccount(It.IsAny<SimpleAccountDto>())).Throws(new Exception());
            HttpResponseMessage result = _accountController.CreateAccount(_simpleAccountDto);

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }

        #endregion

        #region UpdateAccount

        [TestMethod]
        public void UpdateAccount_Success()
        {
            _accountService.Setup(x => x.UpdateAccount(It.IsAny<Guid>(), It.IsAny<UpdateAccountDto>())).Returns(_accountDto);
            HttpResponseMessage result = _accountController.UpdateAccount(_id, _updateAccountDto);

            result.TryGetContentValue<AccountDto>(out AccountDto accountResult);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(_accountDto.UUID, accountResult.UUID);
        }

        [TestMethod]
        public void UpdateAccount_ArgumentException()
        {
            _accountService.Setup(x => x.UpdateAccount(It.IsAny<Guid>(), It.IsAny<UpdateAccountDto>())).Throws(_argumentException);
            HttpResponseMessage result = _accountController.UpdateAccount(_id, _updateAccountDto);

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void UpdateAccount_Exception()
        {
            _accountService.Setup(x => x.UpdateAccount(It.IsAny<Guid>(), It.IsAny<UpdateAccountDto>())).Throws(new Exception());
            HttpResponseMessage result = _accountController.UpdateAccount(_id, _updateAccountDto);

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }

        #endregion

        #region DeleteAccount

        [TestMethod]
        public void DeleteAccount_Success()
        {
            _accountService.Setup(x => x.DeleteAccount(It.IsAny<Guid>()));
            HttpResponseMessage result = _accountController.DeleteAccount(_id);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void DeleteAccount_ArgumentException()
        {
            _accountService.Setup(x => x.DeleteAccount(It.IsAny<Guid>())).Throws(_argumentException);
            HttpResponseMessage result = _accountController.DeleteAccount(_id);

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void DeleteAccount_Exception()
        {
            _accountService.Setup(x => x.DeleteAccount(It.IsAny<Guid>())).Throws(new Exception());
            HttpResponseMessage result = _accountController.DeleteAccount(_id);

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }

        #endregion
    }
}
