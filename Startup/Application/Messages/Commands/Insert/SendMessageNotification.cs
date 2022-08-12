using MediatR;

namespace Application.Messages.Commands.Insert
{
    public class SendMessageNotification : INotification
    {
        public MessageViewModel Message { get; set; }
    }
}