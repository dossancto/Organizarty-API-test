using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Organizarty.Adapters;
using Organizarty.Domain.Entities;
using Organizarty.Domain.Exceptions;
using Organizarty.Domain.UseCases.Users;
using Organizarty.Infra.Data.Contexts;

namespace Organizarty.Application.app.Services;

public class SignService : ISignUseCase
{
    private readonly TimeSpan TOKEN_MAX_AGE = TimeSpan.FromHours(24);

    private readonly ApplicationDbContext _context;
    private readonly ICryptographys _crypto;
    private readonly ITokenAdapter _token;
    private readonly IEmailSenderAdapter _email;
    private readonly IValidator<User> _userValidator;

    public SignService(ApplicationDbContext context, ICryptographys crypto, ITokenAdapter token, IEmailSenderAdapter email, IValidator<User> userValidator)
    {
        _context = context;
        _crypto = crypto;
        _token = token;
        _email = email;
        _userValidator = userValidator;
    }

    public async Task<User> ConfirmEmailCode(string emailCode)
    {
        var emailconfirmation = await _context.UserConfirmEmails
                                            .Include(x => x.User)
                                            .Where(x => x.Id.ToString() == emailCode)
                                            .FirstOrDefaultAsync();

        if (emailconfirmation is null)
        {
            throw new Exception("Email code not founded");
        }

        if (DateTime.Now >= emailconfirmation.ValidFor)
        {
            throw new Exception("Email code expired");
        }

        var user = emailconfirmation.User;

        if (user is null)
        {
            throw new Exception("Why is user null here?");
        }

        user.EmailConfirmed = true;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        var emailCodes = await _context.UserConfirmEmails.Where(ec => ec.User.Id == user.Id).ToListAsync();
        _context.UserConfirmEmails.RemoveRange(emailCodes);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task SendEmailConfirmCode(string targetEmail)
    {
        var user = await _context.Users
          .Select(x => new User
          {
              Id = x.Id,
              Email = x.Email
          })
          .FirstOrDefaultAsync(user => user.Email == targetEmail);

        if (user is null)
        {
            throw new Exception("User not found");
        }

        var confirmCode = new UserConfirmEmail
        {
            UserId = user.Id,
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
            throw new ValidationFailException("Pleace inform an Email and Password");
        }

        var storedUser = await _context.Users.FirstOrDefaultAsync(user => user.Email == email);

        if (storedUser is null)
        {
            throw new NotFoundException($"User with email '{email}' not found. Consider Create a new Account.");
        }

        bool isCredentialsValid = _crypto.VerifyPassword(password, storedUser.Password ?? "", storedUser.Salt ?? "");

        if (!isCredentialsValid)
        {
            throw new NotFoundException("Email or password is not valid.");
        }

        if (!storedUser.EmailConfirmed)
        {
            throw new NotFoundException($"Pleace Confirm your email");
        }

        var token = _token.GenerateToken(storedUser.Id.ToString(), email);

        return (storedUser, token);
    }

    public async Task<User> Register(string userName, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ValidationFailException("Password cant be blank");
        }

        (string hashedPassword, string salt) = _crypto.HashPassword(password);

        var user = new User
        {
            UserName = userName,
            Email = email,
            Password = hashedPassword,
            Salt = salt
        };

        var result = await _userValidator.ValidateAsync(user);

        if (!result.IsValid)
        {
            throw new ValidationFailException(result.ToString());
        }

        var savedUser = _context.Users.Add(user);
        await _context.SaveChangesAsync();

        await SendEmailConfirmCode(email);

        return savedUser.Entity;
    }

    public Task<ThirdParty> ThirdPartyRegister(string name, string description, string loginEmail, string password, string profissionalPhone, string contactEmail, string contactPhone, string cnpj, List<string> tags)
    {
        throw new NotImplementedException();
    }

    public Task<(ThirdParty User, string Token)> ThirdPartyLogin(string loginEmail, string password)
    {
        throw new NotImplementedException();
    }
}
