using Application.Common.Security;
using MediatR;

namespace Application.Users.Commands.Delete
{
    [Authorize(Policy = "RequireAdminRole")]
    public class DeleteUserCommand : IRequest
    {
        public string Id { get; set; }
    }
}