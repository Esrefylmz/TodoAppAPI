using TodoAppCore.Models;

namespace TodoAppApplication.Abstractions
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}