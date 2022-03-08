using MediatR;

namespace Application.Auth.Commands.Register
{
    public class RegisterCommand : IRequest
    {
        public string ClientUri { get; set; }
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }
}