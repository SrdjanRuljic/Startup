using MediatR;

namespace Application.Auth.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest
    {
        public string Email { get; set; }
        public string ClientUri { get; set; }
    }
}