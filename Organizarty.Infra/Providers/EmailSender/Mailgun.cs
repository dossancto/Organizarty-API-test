using System.Net.Http.Headers;
using System.Text;

using Organizarty.Adapters;
using Organizarty.Domain.Exceptions;
using Organizarty.Infra.Providers.EmailSender.Configuration;

namespace Organizarty.Infra.Providers.EmailSender;

public class Mailgun : IEmailSenderAdapter
{
    private readonly MailgunConfiguration _emailconfiguration;

    public Mailgun(MailgunConfiguration mailgunConfiguration)
    {
        _emailconfiguration = mailgunConfiguration;
    }

    public async Task SendConfirmationCode(string targetEmail, string code)
    {
        using (var httpClient = new HttpClient())
        {
            var authToken = Encoding.ASCII.GetBytes($"api:{_emailconfiguration.ApiKey}");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

            var formContent = new FormUrlEncodedContent(new Dictionary<string, string> {
        { "from", $"Mailgun Sandbox <postmaster@{_emailconfiguration.Domain}>"},
        { "h:Reply-To", $"{_emailconfiguration.DisplayName} <{_emailconfiguration.ReplyTo}>" },
        { "to", targetEmail },
        { "subject", "Email confirm" },
        { "text", $"Place confirm your email using this code {code}" }
    });

            var result = await httpClient.PostAsync($"https://api.mailgun.net/v3/{_emailconfiguration.Domain}/messages", formContent);

            if (!result.IsSuccessStatusCode)
            {
                string responseBody = await result.Content.ReadAsStringAsync();
                throw new EmailsenderException(responseBody);
            }
        }
    }
}
