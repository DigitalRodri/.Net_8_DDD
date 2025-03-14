using ApplicationCore.Helpers;
using Domain.DTOs;
using Domain.Helpers;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace ApplicationCore.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController(IAccountService accountService, ILogger<AccountController> logger) : ControllerBase
    {

        [Authorize]
        [HttpGet()]
        public IActionResult GetAllAccounts()
        {
            try
            {
                Response<IEnumerable<AccountDto>> response = accountService.GetAllAccounts();

                if (!response.Content.IsNullOrEmpty() && !response.Content.Any())
                    return response.CreateHttpResponse(System.Net.HttpStatusCode.NoContent);

                return response.CreateHttpResponse();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetInternalServerErrorMessage());
            }
        }

        [Authorize]
        [HttpGet("{uuid}")]
        public ActionResult<AccountDto> GetAccount(Guid uuid)
        {
            try
            {
                Response<AccountDto> response = accountService.GetAccount(uuid);

                if (response.Content == null)
                    return response.CreateHttpResponse(System.Net.HttpStatusCode.NoContent);

                return response.CreateHttpResponse();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetInternalServerErrorMessage());
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult<AccountDto> CreateAccount(SimpleAccountDto simpleAccountDto)
        {
            try
            {
                var accountDto = accountService.CreateAccount(simpleAccountDto);
                return Created(new Uri(Request.GetEncodedUrl() + "/" + accountDto.UUID), accountDto);
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetBadRequestErrorMessage());
            }
            catch (DuplicateNameException ex)
            {
                logger.LogWarning(ex.ToString());
                return Conflict(ex);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetInternalServerErrorMessage());
            }
        }

        [Authorize]
        [HttpPut("{uuid}")]
        public ActionResult<AccountDto> UpdateAccount(Guid uuid, UpdateAccountDto updateAccountDto)
        {
            try
            {
                var modifiedAccount = accountService.UpdateAccount(uuid, updateAccountDto);
                return Ok(modifiedAccount);
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetBadRequestErrorMessage());
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetInternalServerErrorMessage());
            }
        }

        [Authorize]
        [HttpDelete("{uuid}")]
        public IActionResult DeleteAccount(Guid uuid)
        {
            try
            {
                accountService.DeleteAccount(uuid);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetBadRequestErrorMessage());
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetInternalServerErrorMessage());
            }
        }

        [HttpPost()]
        [Route("authentication")]
        public ActionResult<string> Authenticate(AuthenticationDto authenticationDto)
        {
            try
            {
                Response<string> result = accountService.Authenticate(authenticationDto);

                return result.CreateHttpResponse();
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetBadRequestErrorMessage());
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetInternalServerErrorMessage());
            }
        }

    }
}
