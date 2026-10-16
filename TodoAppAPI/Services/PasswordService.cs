using Microsoft.AspNetCore.Identity;
using TodoAppApplication.Abstractions;
using TodoAppCore.Models;

namespace TodoAppAPI.Services;

public class PasswordService : IPasswordService
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public PasswordService(IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(
        User user,
        string hashedPassword,
        string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(
            user,
            hashedPassword,
            providedPassword);

        return result != PasswordVerificationResult.Failed;
    }
}