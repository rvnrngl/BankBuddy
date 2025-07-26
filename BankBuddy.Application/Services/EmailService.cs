using BankBuddy.Application.Interfaces.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.Services
{
    public class EmailService(IConfiguration configuration) : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            IConfigurationSection smtp_creds = configuration.GetSection("Smtp");

            using SmtpClient client = new(smtp_creds["Host"], int.Parse(smtp_creds["Port"]!))
            {
                Credentials = new NetworkCredential(smtp_creds["Username"], smtp_creds["Password"]),
                EnableSsl = bool.Parse(smtp_creds["EnableSsl"]!)
            };

            MailMessage mail = new()
            {
                From = new MailAddress(smtp_creds["Username"]!, "BankBuddy"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(email);
            await client.SendMailAsync(mail);
        }
    }
}
