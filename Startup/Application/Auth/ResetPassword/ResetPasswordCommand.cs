using Application.Common.Security;
using MediatR;

namespace Application.Auth.ResetPassword
{
    [Authorize(Policy = "RequireAuthorization")]
    public class ResetPasswordCommand : IRequest
    {
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}