using InCinema.Models;
using InCinema.Models.Configurations;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace InCinema.Services;

public class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;
    private readonly CompanyData _companyData;

    public EmailService(IOptions<EmailOptions> emailOptions, IOptions<CompanyData> companyData)
    {
        _emailOptions = emailOptions.Value;
        _companyData = companyData.Value;
    }

    public void Send(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_companyData.Name, _companyData.Email));
        emailMessage.To.Add(new MailboxAddress(string.Empty, email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(TextFormat.Html)
        {
            Text = message
        };

        using var client = new SmtpClient();
        client.Connect(_emailOptions.Host, _emailOptions.Port, _emailOptions.IsNeedSsl);
        client.Authenticate(_companyData.Email, _companyData.EmailPassword);
        client.Send(emailMessage);

        client.Disconnect(true);
    }
}