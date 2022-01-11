using MediatR;

namespace Application.Users.Commands.ChangeUserPassword
{
    public class ChangeUserPasswordCommand : IRequest
    {
        public string Id { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmedPassword { get; set; }
    }
}
