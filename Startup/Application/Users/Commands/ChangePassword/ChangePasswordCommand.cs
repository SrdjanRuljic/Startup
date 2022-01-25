using Application.Common.Security;
using MediatR;

namespace Application.Users.Commands.ChangePassword
{
    [Authorize(Policy = "RequireAuthorization")]
    public class ChangePasswordCommand : IRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmedPassword { get; set; }
    }
}