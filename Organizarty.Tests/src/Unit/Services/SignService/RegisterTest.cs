using Organizarty.Application.app.Services;

namespace Organizarty.Tests.src.Unit.Services;

public class RegisterTest : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly SignService _signService;

    public RegisterTest()
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

    [Fact]
    public async Task Register_Validinfos_ReturnCreatedUser()
    {
        var user = await _signService.Register("john123", "valid@email.com", "secure_and_long_password");

        Assert.NotEqual(Guid.Empty, user.Id);
    }

    [Fact]
    public async Task Register_InvalidName_ReturnValidationFailException()
    {
        await Assert.ThrowsAsync<ValidationFailException>(async () =>
        {
            await _signService.Register("john", "valid@email.com", "secure_and_long_password");
        });
    }

    [Fact]
    public async Task Register_InvalidEmail()
    {
        await Assert.ThrowsAsync<ValidationFailException>(async () =>
        {
            await _signService.Register("john123", "invalid_email.com", "secure_and_long_password");
        });
    }
}
