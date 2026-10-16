using TodoAppApplication.DTOs.Auth;

namespace TodoAppApplication.Services
{
    public interface IAuthService
    {
        void Register(RegisterDto registerDto);
        AuthResponseDto Login(LoginDto loginDto);
    }
}