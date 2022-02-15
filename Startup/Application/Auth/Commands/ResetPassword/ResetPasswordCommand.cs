using Application.Common.Security;
using MediatR;

namespace Application.Auth.Commands.ResetPassword
{
    [Authorize(Policy = "RequireAuthorization")]
    public class ResetPasswordCommand : IRequest
    {
        public string ConfirmedPassword { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}