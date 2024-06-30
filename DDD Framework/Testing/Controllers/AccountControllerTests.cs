using Application.Controllers;
using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data;
using System.Net;
using Testing.Helpers;

namespace Testing.Services
{
    [TestClass]
    public class AccountControllerTests
    {
        private AccountController _accountController;
        private Mock<IAccountService> _accountService;
        private Mock<HttpRequest> _httpRequest;
        private HttpContext _httpContext;
        private ControllerContext _controllerContext;

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
            _httpRequest = new Mock<HttpRequest>();

            _httpRequest.Setup(x => x.Scheme).Returns("http");
            _httpRequest.Setup(x => x.Host).Returns(HostString.FromUriComponent("localhost:8080"));
            _httpRequest.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api"));

            _httpContext = Mock.Of<HttpContext>(x => x.Request == _httpRequest.Object);
            _controllerContext = new ControllerContext(){ HttpContext = _httpContext };

            _accountController = new AccountController(_accountService.Object) { ControllerContext = _controllerContext };

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
            ActionResult<AccountDto> result = _accountController.GetAccount(_id);

            OkObjectResult objectResult = result.Result as OkObjectResult;
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);
            AccountDto accountDtoResult = objectResult.Value as AccountDto;
            Assert.AreEqual(_accountDto.UUID, accountDtoResult.UUID);
        }

        [TestMethod]
        public void GetAccount_ArgumentException()
        {
            _accountService.Setup(x => x.GetAccount(It.IsAny<Guid>())).Throws(_argumentException);
            ActionResult<AccountDto> result = _accountController.GetAccount(_id);

            BadRequestObjectResult objectResult = result.Result as BadRequestObjectResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        }

        [TestMethod]
        public void GetAccount_Exception()
        {
            _accountService.Setup(x => x.GetAccount(It.IsAny<Guid>())).Throws(new Exception());
            ActionResult<AccountDto> result = _accountController.GetAccount(_id);

            ObjectResult objectResult = result.Result as ObjectResult;
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        }

        #endregion

        #region CreateAccount

        [TestMethod]
        public void CreateAccount_Success()
        {
            _accountService.Setup(x => x.CreateAccount(It.IsAny<SimpleAccountDto>())).Returns(_accountDto);
            ActionResult<AccountDto> result = _accountController.CreateAccount(_simpleAccountDto);

            ObjectResult objectResult = result.Result as ObjectResult;
            Assert.AreEqual((int)HttpStatusCode.Created, objectResult.StatusCode);
            AccountDto accountDtoResult = objectResult.Value as AccountDto;
            Assert.AreEqual(_accountDto.UUID, accountDtoResult.UUID);
        }

        [TestMethod]
        public void CreateAccount_ArgumentException()
        {
            _accountService.Setup(x => x.CreateAccount(It.IsAny<SimpleAccountDto>())).Throws(_argumentException);
            ActionResult<AccountDto> result = _accountController.CreateAccount(_simpleAccountDto);

            BadRequestObjectResult objectResult = result.Result as BadRequestObjectResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        }

        [TestMethod]
        public void CreateAccount_DuplicateNameException()
        {
            _accountService.Setup(x => x.CreateAccount(It.IsAny<SimpleAccountDto>())).Throws(_duplicateNameException);
            ActionResult<AccountDto> result = _accountController.CreateAccount(_simpleAccountDto);

            ConflictObjectResult statusCode = result.Result as ConflictObjectResult;
            Assert.AreEqual((int)HttpStatusCode.Conflict, statusCode.StatusCode);
        }

        [TestMethod]
        public void CreateAccount_Exception()
        {
            _accountService.Setup(x => x.CreateAccount(It.IsAny<SimpleAccountDto>())).Throws(new Exception());
            ActionResult<AccountDto> result = _accountController.CreateAccount(_simpleAccountDto);

            ObjectResult objectResult = result.Result as ObjectResult;
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        }

        #endregion

        #region UpdateAccount

        [TestMethod]
        public void UpdateAccount_Success()
        {
            _accountService.Setup(x => x.UpdateAccount(It.IsAny<Guid>(), It.IsAny<UpdateAccountDto>())).Returns(_accountDto);
            ActionResult<AccountDto> result = _accountController.UpdateAccount(_id, _updateAccountDto);

            OkObjectResult objectResult = result.Result as OkObjectResult;
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);
            AccountDto accountDtoResult = objectResult.Value as AccountDto;
            Assert.AreEqual(_accountDto.UUID, accountDtoResult.UUID);
        }

        [TestMethod]
        public void UpdateAccount_ArgumentException()
        {
            _accountService.Setup(x => x.UpdateAccount(It.IsAny<Guid>(), It.IsAny<UpdateAccountDto>())).Throws(_argumentException);
            ActionResult<AccountDto> result = _accountController.UpdateAccount(_id, _updateAccountDto);
            BadRequestObjectResult objectResult = result.Result as BadRequestObjectResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        }

        [TestMethod]
        public void UpdateAccount_Exception()
        {
            _accountService.Setup(x => x.UpdateAccount(It.IsAny<Guid>(), It.IsAny<UpdateAccountDto>())).Throws(new Exception());
            ActionResult<AccountDto> result = _accountController.UpdateAccount(_id, _updateAccountDto);

            ObjectResult objectResult = result.Result as ObjectResult;
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        }

        #endregion

        #region DeleteAccount

        [TestMethod]
        public void DeleteAccount_Success()
        {
            _accountService.Setup(x => x.DeleteAccount(It.IsAny<Guid>()));
            IActionResult result = _accountController.DeleteAccount(_id);

            Assert.AreEqual((int)HttpStatusCode.OK, ((IStatusCodeActionResult)result).StatusCode);
        }

        [TestMethod]
        public void DeleteAccount_ArgumentException()
        {
            _accountService.Setup(x => x.DeleteAccount(It.IsAny<Guid>())).Throws(_argumentException);
            IActionResult result = _accountController.DeleteAccount(_id);

            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((IStatusCodeActionResult)result).StatusCode);
        }

        [TestMethod]
        public void DeleteAccount_Exception()
        {
            _accountService.Setup(x => x.DeleteAccount(It.IsAny<Guid>())).Throws(new Exception());
            IActionResult result = _accountController.DeleteAccount(_id);

            Assert.AreEqual((int)HttpStatusCode.InternalServerError, ((IStatusCodeActionResult)result).StatusCode);
        }

        #endregion
    }
}
