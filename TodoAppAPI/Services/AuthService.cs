using Microsoft.AspNetCore.Identity;
using TodoAppAPI.DTOs.Auth;
using TodoAppAPI.ExceptionHandling.Exceptions;
using TodoAppCore.Models;
using TodoAppCore.Repositories;

namespace TodoAppAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
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

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                loginDto.Password);

            if (result == PasswordVerificationResult.Failed)
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
                _passwordHasher.HashPassword(user, registerDto.Password);

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