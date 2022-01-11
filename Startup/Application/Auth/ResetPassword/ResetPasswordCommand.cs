using MediatR;

namespace Application.Auth.ResetPassword
{
    public class ResetPasswordCommand : IRequest
    {
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}