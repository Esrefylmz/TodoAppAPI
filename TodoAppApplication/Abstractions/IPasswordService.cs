using TodoAppCore.Models;

namespace TodoAppApplication.Abstractions;

public interface IPasswordService
{
    string HashPassword(User user, string password);

    bool VerifyPassword(
        User user,
        string hashedPassword,
        string providedPassword);
}