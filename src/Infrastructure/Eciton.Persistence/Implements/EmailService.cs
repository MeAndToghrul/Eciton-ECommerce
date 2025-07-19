using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Eciton.Application.Abstractions;
using Eciton.Application.Options;
using Eciton.Application.ResponceObject;
using Eciton.Application.ResponceObject.Enums;

public class EmailService : IEmailService
{
    private readonly SmtpOptions _options;

    public EmailService(IOptions<SmtpOptions> options)
    {
        _options = options.Value;
    }    

    public async Task<Response> SendVerificationEmailAsync(string email, string token)
    {
        var verifyUrl = $"https://localhost:7029/api/auth/verify-email?token={token}";

        var mail = new MailMessage
        {
            From = new MailAddress(_options.Sender),
            Subject = "E-poçt Doğrulaması",
            Body = $@"
        <div class='email-wrapper' style='
          max-width: 620px;
          margin: 40px auto;
          background-color: #ffffff;
          border-radius: 10px;
          box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
          overflow: hidden;
          font-family: Segoe UI, Tahoma, Geneva, Verdana, sans-serif;
        '>
          <div class='email-header' style='
            background-color: #007bff;
            color: #fff;
            padding: 20px;
            text-align: center;
          '>
            <h1 style='
              margin: 0;
              font-size: 24px;
              font-weight: 600;
            '>📨 E-poçt Doğrulama</h1>
          </div>
          <div class='email-body' style='
            padding: 30px 40px;
            text-align: center;
          '>
            <p style='
              color: #444;
              font-size: 16px;
              line-height: 1.6;
              margin-bottom: 25px;
            '>
              Salam,<br />
              Qeydiyyat prosesini tamamlamaq üçün zəhmət olmasa aşağıdakı düyməyə klikləyərək e-poçt ünvanınızı təsdiqləyin.
            </p>
            <a href='{verifyUrl}' class='btn' style='
              display: inline-block;
              padding: 14px 30px;
              background-color: #28a745;
              color: #fff;
              text-decoration: none;
              border-radius: 6px;
              font-weight: 600;
              font-size: 17px;
              transition: all 0.3s ease;
            ' target='_blank'>
              E-poçtumu Təsdiqlə
            </a>
            <p style='margin-top: 30px; color: #999;'>
              Əgər bu sorğunu siz etməmisinizsə, bu məktubu sadəcə nəzərə almayın.
            </p>
          </div>
          <div class='email-footer' style='
            background-color: #f0f0f0;
            padding: 25px;
            font-size: 13px;
            color: #666;
            text-align: center;
          '>
            Bu mesaj <strong>Eciton</strong> tərəfindən göndərilmişdir.<br />
            Suallar üçün: <a href='mailto:backend.dev.net@gmail.com' style='color: #007bff;'>backend.dev.net@gmail.com</a><br />
            &copy; 2025 Eciton. Bütün hüquqlar qorunur.
          </div>
        </div>
        ",
            IsBodyHtml = true
        };

        mail.To.Add(email);

        using var smtp = new SmtpClient(_options.Host, _options.Port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(_options.Sender, _options.Password)
        };

        await smtp.SendMailAsync(mail);
        return new Response(ResponseStatusCode.Success, "Verification email sent successfully.");
    }
}
