using MediatR;

namespace Application.Auth.Queries.Login
{
    public class LoginQuery : IRequest<object>
    {
        public string Password { get; set; }
        public string Username { get; set; }
    }
}