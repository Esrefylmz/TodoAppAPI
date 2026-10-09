using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TodoAppAPI.DTOs.Auth;
using TodoAppAPI.Services;

namespace TodoAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;

        public AuthController(
            IAuthService authService,
            IValidator<RegisterDto> registerValidator,
            IValidator<LoginDto> loginValidator)
        {
            _authService = authService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto registerDto)
        {
            _registerValidator.ValidateAndThrow(registerDto);

            _authService.Register(registerDto);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto loginDto)
        {
            _loginValidator.ValidateAndThrow(loginDto);

            var result = _authService.Login(loginDto);

            return Ok(result);
        }
    }
}