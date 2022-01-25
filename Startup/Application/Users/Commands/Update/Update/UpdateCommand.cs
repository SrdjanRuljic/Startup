using Application.Common.Security;
using MediatR;

namespace Application.Users.Commands.Update.Update
{
    [Authorize(Policy = "RequireAdminRole")]
    public class UpdateCommand : IRequest
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
    }
}