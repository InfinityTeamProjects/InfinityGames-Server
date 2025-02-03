using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using UserService.Application.Abstractions;

namespace UserService.Infrastructure.Services;

public class EmailService(IConfiguration configuration) : IEmailService
{
    private readonly IConfiguration _configuration = configuration;

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        using var smtp = new SmtpClient()
        {
            Host = _configuration["EmailSettings:SmtpHost"],
            Port = int.Parse(_configuration["EmailSettings:SmtpPort"]),
            EnableSsl = true,
            Credentials = new NetworkCredential(
                _configuration["EmailSettings:SmtpUser"],
                _configuration["EmailSettings:SmtpPass"])
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["EmailSettings:FromEmail"], _configuration["EmailSettings:UserName"]),
            Subject = subject,
            Body = message,
            IsBodyHtml = true
        };
        mailMessage.To.Add(email);

        await smtp.SendMailAsync(mailMessage);
    }
}
