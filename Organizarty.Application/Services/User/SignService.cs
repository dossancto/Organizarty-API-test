using Microsoft.EntityFrameworkCore;

using Organizarty.Adapters;
using Organizarty.Domain.Entities;
using Organizarty.Domain.UseCases.Users;
using Organizarty.Infra.Data.Contexts;

namespace Organizarty.Application.Services;

public class SignService : ISignUseCase
{
    private readonly TimeSpan TOKEN_MAX_AGE = TimeSpan.FromHours(24);

    private readonly ApplicationDbContext _context;
    private readonly ICryptographys _crypto;
    private readonly ITokenAdapter _token;
    private readonly IEmailSenderAdapter _email;

    public SignService(ApplicationDbContext context, ICryptographys crypto, ITokenAdapter token, IEmailSenderAdapter email)
    {
        _context = context;
        _crypto = crypto;
        _token = token;
        _email = email;
    }

    public async Task<User> ConfirmEmailCode(string emailCode)
    {
        var emailconfirmation = await _context.UserConfirmEmails
                                            .Include(x => x.User)
                                            .Where(x => x.Id.ToString() == emailCode)
                                            .FirstOrDefaultAsync();

        if (emailconfirmation is null)
            throw new Exception("Email code not founded");

        if (DateTime.Now >= emailconfirmation.ValidFor)
            throw new Exception("Email code expired");

        var user = emailconfirmation.User;

        if (user is null)
            throw new Exception("Why is user null here?");

        user.EmailConfirmed = true;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        var emailCodes = await _context.UserConfirmEmails.Where(ec => ec.User.Id == user.Id).ToListAsync();
        _context.UserConfirmEmails.RemoveRange(emailCodes);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task SendEmailConfirmCode(Guid userId, string targetEmail)
    {
        var confirmCode = new UserConfirmEmail
        {
            UserId = userId,
            ValidFor = DateTime.Now.Add(TOKEN_MAX_AGE)
        };

        var savedEmail = _context.UserConfirmEmails.Add(confirmCode);
        await _context.SaveChangesAsync();

        var emailCode = savedEmail.Entity.Id.ToString();

        await _email.SendConfirmationCode(targetEmail, emailCode);
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
        // TODO: Add domain validations
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

        await SendEmailConfirmCode(savedUser.Entity.Id, email);

        return savedUser.Entity;
    }
}
