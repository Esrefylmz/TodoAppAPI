using TodoAppDTOs.Auth;
using TodoAppApplication.Exceptions;
using TodoAppCore.Models;
using TodoAppCore.Repositories;
using TodoAppApplication.Abstractions;

namespace TodoAppApplication.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;

        public AuthService(
            IUserRepository userRepository,
            IPasswordService passwordService,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        private User ValidateCredentials(LoginDto loginDto)
        {
            var user = _userRepository.GetByUsername(loginDto.Username);

            if (user == null)
            {
                throw new UnauthorizedException(
                    "Invalid username or password.");
            }

            var isPasswordValid = _passwordService.VerifyPassword(
                user,
                user.PasswordHash,
                loginDto.Password);

            if (!isPasswordValid)
            {
                throw new UnauthorizedException(
                    "Invalid username or password.");
            }


            return user;
        }

        public void Register(RegisterDto registerDto)
        {
            if (_userRepository.ExistsByUsername(registerDto.Username))
            {
                throw new ConflictException(
                    $"Username '{registerDto.Username}' is already taken.");
            }

            var user = new User
            {
                Username = registerDto.Username
            };

            user.PasswordHash =
                _passwordService.HashPassword(user, registerDto.Password);

            _userRepository.Add(user);
        }

        public AuthResponseDto Login(LoginDto loginDto)
        {
            var user = ValidateCredentials(loginDto);

            var token = _tokenService.CreateToken(user);

            return new AuthResponseDto
            {
                Token = token
            };
        }
    }
}