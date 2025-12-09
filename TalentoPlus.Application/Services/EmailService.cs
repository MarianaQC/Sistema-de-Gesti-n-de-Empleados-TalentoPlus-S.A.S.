using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using TalentoPlus.Application.Interfaces;

namespace TalentoPlus.Application.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task SendWelcomeEmailAsync(string toEmail, string employeeName)
    {
        var smtpHost = _configuration["Email:SmtpHost"] ?? "smtp.gmail.com";
        var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
        var smtpUser = _configuration["Email:SmtpUser"] ?? "";
        var smtpPassword = _configuration["Email:SmtpPassword"] ?? "";
        var fromEmail = _configuration["Email:FromEmail"] ?? smtpUser;
        
        var message = new MailMessage
        {
            From = new MailAddress(fromEmail, "TalentoPlus S.A.S."),
            Subject = "¡Bienvenido a TalentoPlus!",
            Body = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2 style='color: #2196F3;'>¡Bienvenido a TalentoPlus S.A.S.!</h2>
                    <p>Estimado(a) <strong>{employeeName}</strong>,</p>
                    <p>Su registro ha sido completado exitosamente.</p>
                    <p>Ya puede autenticarse en nuestra plataforma cuando esté habilitado.</p>
                    <br/>
                    <p>Saludos cordiales,</p>
                    <p><strong>Equipo de Recursos Humanos</strong></p>
                    <p>TalentoPlus S.A.S.</p>
                </body>
                </html>
            ",
            IsBodyHtml = true
        };
        
        message.To.Add(new MailAddress(toEmail));
        
        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUser, smtpPassword),
            EnableSsl = true
        };
        
        await client.SendMailAsync(message);
    }
}