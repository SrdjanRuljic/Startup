using MediatR;

namespace Application.Users.Commands.Insert.Notification
{
    public class SendPasswordEmailNotification : INotification
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}