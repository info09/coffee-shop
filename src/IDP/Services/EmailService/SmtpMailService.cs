using IDP.Common;
using System.Net;
using System.Net.Mail;

namespace IDP.Services.EmailService;

public class SmtpMailService : IEmailSender
{
    private readonly SMTPEmailSetting _smtpEmailSetting;
    public SmtpMailService(SMTPEmailSetting smtpEmailSetting)
    {
        _smtpEmailSetting = smtpEmailSetting;
    }

    public void SendEmail(string recipient, string subject, string body, bool isBodyHtml = false, string sender = null)
    {
        var message = new MailMessage(_smtpEmailSetting.From, recipient)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = isBodyHtml,
            From = new MailAddress(_smtpEmailSetting.From, sender ?? _smtpEmailSetting.From)
        };

        using var client = new SmtpClient(_smtpEmailSetting.SMTPServer, _smtpEmailSetting.Port)
        {
            EnableSsl = _smtpEmailSetting.UseSsl
        };

        if (!string.IsNullOrEmpty(_smtpEmailSetting.Username) && !string.IsNullOrEmpty(_smtpEmailSetting.Password))
        {
            client.Credentials = new NetworkCredential(_smtpEmailSetting.Username, _smtpEmailSetting.Password);
        }
        else
        {
            client.UseDefaultCredentials = true;
        }

        client.Send(message);
    }
}
