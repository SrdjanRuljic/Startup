using MediatR;

namespace Application.Auth.Commands.Login
{
    public class LoginCommand : IRequest<object>
    {
        public string Password { get; set; }
        public string Username { get; set; }
    }
}