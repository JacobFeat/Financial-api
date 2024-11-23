using System.Data;
using Dapper;
using FinancialApi.Data;
using FinancialApi.Dtos;
using FinancialApi.Helpers;
using FinancialApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace FinancialApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserForRegistrationsDto userForRegistrationsDto)
        {
            try
            {
                await _authService.RegisterUserAsync(userForRegistrationsDto);
                return Ok("Registration successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            try
            {
                var loginSuccessful = await _authService.LoginUserAsync(userForLoginDto);

                if (loginSuccessful)
                {
                    return Ok();
                }

                return Unauthorized("Invalid email or password.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}