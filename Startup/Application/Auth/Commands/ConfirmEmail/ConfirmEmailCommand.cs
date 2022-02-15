using MediatR;

namespace Application.Auth.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest
    {
        public string Token { get; set; }
        public string UserName { get; set; }
    }
}