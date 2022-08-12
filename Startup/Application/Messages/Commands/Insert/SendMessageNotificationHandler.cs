using Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Messages.Commands.Insert
{
    public class SendMessageNotificationHandler : INotificationHandler<SendMessageNotification>
    {
        private readonly IMessageHub _messageHub;

        public SendMessageNotificationHandler(IMessageHub messageHub)
        {
            _messageHub = messageHub;
        }

        public async Task Handle(SendMessageNotification notification, CancellationToken cancellationToken)
        {
            await _messageHub.SendMessage(notification.Message);
        }
    }
}