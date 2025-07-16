using Eciton.Application.ResponceObject;
using System.Net.Mail;

namespace Eciton.Application.Abstractions;
public interface IEmailService
{
    Task<Response> SendEmailAsync(MailMessage msg);
}
