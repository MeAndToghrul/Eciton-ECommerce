using Eciton.Application.ResponceObject;

namespace Eciton.Application.Abstractions;
public interface IEmailService
{
    Task<Response> SendVerificationEmailAsync(string email, string token);
    Task<Response> SendPasswordResetEmailAsync(string email, string token);
}
