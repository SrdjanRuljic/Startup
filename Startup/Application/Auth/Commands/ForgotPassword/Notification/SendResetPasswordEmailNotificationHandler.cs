using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands.ForgotPassword.Notification
{
    public class SendResetPasswordEmailNotificationHandler : INotificationHandler<SendResetPasswordEmailNotification>
    {
        private readonly IEmailSenderService _emailSenderService;

        public SendResetPasswordEmailNotificationHandler(IEmailSenderService emailSenderService)
        {
            _emailSenderService = emailSenderService;
        }

        public async Task Handle(SendResetPasswordEmailNotification notification, CancellationToken cancellationToken)
        {
            Message message = new Message(new string[] { notification.Email }, "Reset password request", notification.Link, null);

            await _emailSenderService.SendResetPasswordEmailAsync(message);
        }
    }
}