using Organizarty.Adapters;

namespace Organizarty.Tests.src.Mock;

public class EmailSenderAdapterMock : IEmailSenderAdapter
{
    public async Task SendConfirmationCode(string targetEmail, string code) { }
}
