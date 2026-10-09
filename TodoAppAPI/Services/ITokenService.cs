using TodoAppCore.Models;

namespace TodoAppAPI.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}