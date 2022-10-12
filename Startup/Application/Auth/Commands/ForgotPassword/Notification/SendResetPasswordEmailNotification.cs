using MediatR;

namespace Application.Auth.Commands.ForgotPassword.Notification
{
    public class SendResetPasswordEmailNotification : INotification
    {
        public string Email { get; set; }
        public string Link { get; set; }
    }
}