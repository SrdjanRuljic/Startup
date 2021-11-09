using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Exceptions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace Persistence.EmailSender
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly EmailConfiguration _emailConfiguration;
        private readonly ILogger<EmailSenderService> _logger;

        public EmailSenderService(EmailConfiguration emailConfiguration,
                                  ILogger<EmailSenderService> logger)
        {
            _emailConfiguration = emailConfiguration;
            _logger = logger;
        }

        public async Task SendConfirmationEmailAsync(Message message)
        {
            MimeMessage mimeMessage = CreateConfirmationEmailMessage(message);

            await SendAsync(mimeMessage);
        }

        public async Task SendPasswordEmailAsync(Message message)
        {
            MimeMessage mimeMessage = CreateEmailPasswordMessage(message);

            await SendAsync(mimeMessage);
        }

        private MimeMessage CreateConfirmationEmailMessage(Message message)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            BodyBuilder bodyBuilder = new BodyBuilder
            {
                HtmlBody = string.Format("Please confirm your account by <a href = '{0}'>clicking here</a>", message.Link)
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }

        private MimeMessage CreateEmailPasswordMessage(Message message)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            BodyBuilder bodyBuilder = new BodyBuilder
            {
                HtmlBody = string.Format("Your generated password: {0}", message.Password)
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }

        private async Task SendAsync(MimeMessage mimeMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, SecureSocketOptions.Auto);
                    await client.AuthenticateAsync(_emailConfiguration.UserName, _emailConfiguration.Password);
                    await client.SendAsync(mimeMessage);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ErrorMessages.EmailNotSend);
                    //throw new HttpStatusCodeException(HttpStatusCode.BadRequest, ErrorMessages.EmailNotSend);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
