using MediatR;

namespace Application.Auth.Commands.Register.Notification
{
    public class SendConfirmationEmailNotification : INotification
    {
        public string Email { get; set; }
        public string Link { get; set; }
    }
}