using MediatR;

namespace Application.Auth.Register
{
    public class RegisterCommand : IRequest<object>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
