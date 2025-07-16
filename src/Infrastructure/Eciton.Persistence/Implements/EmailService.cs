using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Eciton.Application.Options;
using Eciton.Application.ResponceObject;
using Eciton.Application.ResponceObject.Enums;
using Eciton.Application.Abstractions;

namespace Eciton.Persistence.Implements;

public class EmailService : IEmailService
{
    private readonly SmtpOptions _opt;
    private readonly MailAddress _from;

    public EmailService(IOptions<SmtpOptions> opt)
    {
        _opt = opt.Value;
        _from = new MailAddress(_opt.Sender, "Eciton");
    }

    public async Task<Response> SendEmailAsync(MailMessage message)
    {
        message.From = _from;

        using var client = new SmtpClient(_opt.Host, _opt.Port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(_opt.Sender, _opt.Password)
        };

        await client.SendMailAsync(message);
        return new Response(ResponseStatusCode.Success, "Email sent successfully.");
    }

}
