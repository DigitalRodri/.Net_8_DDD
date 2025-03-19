using ApplicationCore.Helpers;
using Domain.DTOs;
using Domain.Helpers;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Controllers
{
    [ApiController]
    [Route("api/test/account")]
    public class TestController(ITestService testService, ILogger<TestController> logger) : ControllerBase
    {

        [HttpGet("{uuid}")]
        public ActionResult<AccountDto> GetAccountTest(Guid uuid)
        {
            try
            {
                Response<AccountDto> response = testService.GetAccountResult(uuid);
                return response.CreateHttpResponse();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, LoggerHelper.GetInternalServerErrorMessage());
            }
        }

        [HttpGet("exception/{uuid}")]
        public ActionResult<AccountDto> GetAccountExceptionTest(Guid uuid)
        {
            try
            {
                AccountDto response = testService.GetAccountException(uuid);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, LoggerHelper.GetInternalServerErrorMessage());
            }
        }

    }
}
