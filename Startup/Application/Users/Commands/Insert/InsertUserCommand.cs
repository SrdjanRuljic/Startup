using MediatR;

namespace Application.Users.Commands.Insert
{
    public class InsertUserCommand : IRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
    }
}
