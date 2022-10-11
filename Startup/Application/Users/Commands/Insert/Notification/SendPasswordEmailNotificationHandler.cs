using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.Insert.Notification
{
    public class SendPasswordEmailNotificationHandler : INotificationHandler<SendPasswordEmailNotification>
    {
        private readonly IEmailSenderService _emailSenderService;

        public SendPasswordEmailNotificationHandler(IEmailSenderService emailSenderService)
        {
            _emailSenderService = emailSenderService;
        }

        public async Task Handle(SendPasswordEmailNotification notification, CancellationToken cancellationToken)
        {
            Message message = new Message(new string[] { notification.Email }, "Password send", null, notification.Password);

            await _emailSenderService.SendConfirmationEmailAsync(message);
        }
    }
}