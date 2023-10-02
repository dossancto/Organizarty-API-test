using Organizarty.Application.app.Services;

namespace Organizarty.Tests.src.Unit.Services;

public class LoginTest : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly SignService _signService;

    public LoginTest()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "Organizarty_test")
              .Options;

        _context = new ApplicationDbContext(options);
        _signService = new SignService(_context, new CryptographysMock(), new TokenAdapterMock(), new EmailSenderAdapterMock(), new UserValidator());
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    private async Task<User> setupUserConfirmed()
    {
        var user = await _signService.Register("john123", "valid@email.com", "secure_and_long_password");

        var code = await _signService.SendEmailConfirmCode(user.Email!);

        return await _signService.ConfirmEmailCode(code);
    }

    private async Task<User> setupUser()
        => await _signService.Register("john123", "valid@email.com", "secure_and_long_password");

    [Fact]
    public async Task Login_Validinfos_ReturnLoginUser()
    {
        var registeredUser = await setupUserConfirmed();

        var (user, token) = await _signService.Login("valid@email.com", "secure_and_long_password");

        Assert.Equal(user.Id, registeredUser.Id);
        Assert.NotNull(token);
    }

    [Fact]
    public async Task Login_EmailNotConfirmed_ReturnNotFoundException()
    {
        await setupUser();

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await _signService.Login("valid@email.com", "secure_and_long_password");
        });
    }

    [Fact]
    public async Task Login_WrongPassword_ReturnNotFoundException()
    {
        await setupUser();

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await _signService.Login("valid@email.com", "wrong_password");
        });
    }

    [Fact]
    public async Task Login_EmailNotFound_ReturnNotFoundException()
    {
        await setupUser();

        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await _signService.Login("unregistered@email.com", "wrong_password");
        });
    }
}
