namespace Organizarty.Adapters;

public interface IEmailSenderAdapter
{
    Task SendConfirmationCode(string targetEmail, string code);
}
