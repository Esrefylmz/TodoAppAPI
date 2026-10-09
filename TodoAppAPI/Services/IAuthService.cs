using TodoAppAPI.DTOs.Auth;

namespace TodoAppAPI.Services
{
    public interface IAuthService
    {
        void Register(RegisterDto registerDto);
        AuthResponseDto Login(LoginDto loginDto);
    }
}