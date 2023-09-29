using Microsoft.EntityFrameworkCore;

using Organizarty.Adapters;
using Organizarty.Domain.Entities;
using Organizarty.Domain.UseCases.Users;
using Organizarty.Infra.Data.Contexts;

namespace Organizarty.Application.Services;

public class SignService : ISignUseCase
{
    private readonly ApplicationDbContext _context;
    private readonly ICryptographys _crypto;
    private readonly ITokenAdapter _token;

    public SignService(ApplicationDbContext context, ICryptographys crypto, ITokenAdapter token)
    {
        _context = context;
        _crypto = crypto;
        _token = token;
    }

    public Task<User> ConfirmEmailCode(string userId, string emailCode)
    {
        throw new NotImplementedException();
    }

    public async Task<(User User, string Token)> Login(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            // TODO: Change to some Domain Exception
            throw new Exception("Pleace inform an Email and Password");
        }

        var storedUser = await _context.Users.FirstOrDefaultAsync(user => user.Email == email);

        if (storedUser is null)
        {
            // TODO: Change to some Domain Exception
            throw new Exception($"User with email '{email}' not found. Consider Create a new Account.");
        }

        bool isCredentialsValid = _crypto.VerifyPassword(password, storedUser.Password ?? "", storedUser.Salt ?? "");

        if (!isCredentialsValid)
        {
            // TODO: Change to some Domain Exception
            throw new Exception("Email or Password wrong");
        }

        var token = _token.GenerateToken(storedUser.Id.ToString(), email);

        return (storedUser, token);
    }

    public async Task<User> Register(string userName, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            // TODO: Change to some Domain Exception
            throw new Exception("Password cant be blank");
        }

        (string hashedPassword, string salt) = _crypto.HashPassword(password);

        var user = new User
        {
            UserName = userName,
            Email = email,
            Password = hashedPassword,
            Salt = salt
        };

        var savedUser = _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return savedUser.Entity;
    }
}
