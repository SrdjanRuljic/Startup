using Application.Common.Security;
using MediatR;

namespace Application.Users.Commands.ChangeUserPassword
{
    [Authorize(Policy = "RequireAdminRole")]
    public class ChangeUserPasswordCommand : IRequest
    {
        public string Id { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmedPassword { get; set; }
    }
}
