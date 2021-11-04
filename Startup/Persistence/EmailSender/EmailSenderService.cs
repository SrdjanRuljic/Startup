using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Exceptions;
using MailKit.Net.Smtp;
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

        public async Task SendEmailAsync(Message message)
        {
            MimeMessage mimeMessage = CreateEmailMessage(message);

            await SendAsync(mimeMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            BodyBuilder bodyBuilder = new BodyBuilder
            {
                HtmlBody = string.Format("<h2 style='color:red;'>{0}</h2>",
                message.Content)
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
                    await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);

                    client.AuthenticationMechanisms.Remove("XOAUTH2");

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
