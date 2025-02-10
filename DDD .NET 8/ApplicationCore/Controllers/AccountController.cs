using ApplicationCore.Helpers;
using Domain.DTOs;
using Domain.Interfaces;
using Domain.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet()]
        public ActionResult<AccountDto> GetAllAccounts()
        {
            try
            {
                IEnumerable<AccountDto> accountDTOList = _accountService.GetAllAccounts();

                if (accountDTOList.Count() == 0) return NoContent();
                return Ok(accountDTOList);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetInternalServerErrorMessage());
            }
        }

        [Authorize]
        [HttpGet("{UUID}")]
        public ActionResult<AccountDto> GetAccount(Guid UUID)
        {
            try
            {
                AccountDto accountDTO = _accountService.GetAccount(UUID);

                if (accountDTO == null) return NoContent();
                return Ok(accountDTO);
            }
            catch (ArgumentException ex)
            {
                _logger.LogCritical(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetBadRequestErrorMessage());
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetInternalServerErrorMessage());
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult<AccountDto> CreateAccount(SimpleAccountDto simpleAccountDto)
        {
            try
            {
                AccountDto accountDTO = _accountService.CreateAccount(simpleAccountDto);
                return Created(new Uri(Request.GetEncodedUrl() + "/" + accountDTO.UUID), accountDTO);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetBadRequestErrorMessage());
            }
            catch (DuplicateNameException ex)
            {
                _logger.LogWarning(ex.ToString());
                return Conflict(ex);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetInternalServerErrorMessage());
            }
        }

        [Authorize]
        [HttpPut("{UUID}")]
        public ActionResult<AccountDto> UpdateAccount(Guid UUID, UpdateAccountDto updateAccountDto)
        {
            try
            {
                AccountDto modifiedAccount = _accountService.UpdateAccount(UUID, updateAccountDto);
                return Ok(modifiedAccount);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetBadRequestErrorMessage());
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetInternalServerErrorMessage());
            }
        }

        [Authorize]
        [HttpDelete("{UUID}")]
        public IActionResult DeleteAccount(Guid UUID)
        {
            try
            {
                _accountService.DeleteAccount(UUID);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetBadRequestErrorMessage());
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetInternalServerErrorMessage());
            }
        }

        [HttpPost()]
        [Route("authentication")]
        public ActionResult<string> Authenticate(AuthenticationDto authenticationDto)
        {
            try
            {
                string result = _accountService.Authenticate(authenticationDto);

                if (String.IsNullOrEmpty(result))
                    return Unauthorized(Resources.IncorrectPassword);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetBadRequestErrorMessage());
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetInternalServerErrorMessage());
            }
        }

    }
}
