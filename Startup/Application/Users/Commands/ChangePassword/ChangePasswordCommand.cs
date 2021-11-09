using MediatR;

namespace Application.Users.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmedPassword { get; set; }
    }
}
