using MediatR;

namespace Application.Users.Commands.Delete
{
    public class DeleteUserCommand : IRequest
    {
        public string Id { get; set; }
    }
}