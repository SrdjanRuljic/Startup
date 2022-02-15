using MediatR;

namespace Application.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest
    {
        public string ClientUri { get; set; }
        public string Email { get; set; }
    }
}