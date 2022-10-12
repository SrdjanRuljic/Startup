using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands.Register.Notification
{
    public class SendConfirmationEmailNotificationHandler : INotificationHandler<SendConfirmationEmailNotification>
    {
        private readonly IEmailSenderService _emailSenderService;

        public SendConfirmationEmailNotificationHandler(IEmailSenderService emailSenderService)
        {
            _emailSenderService = emailSenderService;
        }

        public async Task Handle(SendConfirmationEmailNotification notification, CancellationToken cancellationToken)
        {
            Message message = new Message(new string[] { notification.Email }, "Email confirmation", notification.Link, null);

            await _emailSenderService.SendConfirmationEmailAsync(message);
        }
    }
}