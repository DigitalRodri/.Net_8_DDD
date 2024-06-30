using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Application.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{UUID}")]
        public ActionResult<AccountDto> GetAccount(Guid UUID)
        {
            try
            {
                AccountDto accountDTO = _accountService.GetAccount(UUID);

                if (accountDTO == null) return NoContent();
                return accountDTO; 
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost]
        public ActionResult<AccountDto> CreateAccount(SimpleAccountDto simpleAccountDto)
        {
            try
            {
                AccountDto accountDTO = _accountService.CreateAccount(simpleAccountDto);
                return accountDTO;
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (DuplicateNameException ex)
            {
                return Conflict(ex);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPut("{UUID}")]
        public ActionResult<AccountDto> UpdateAccount(Guid UUID, UpdateAccountDto updateAccountDto)
        {
            try
            {
                AccountDto modifiedAccount = _accountService.UpdateAccount(UUID, updateAccountDto);
                return modifiedAccount;
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

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
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

    }
}
