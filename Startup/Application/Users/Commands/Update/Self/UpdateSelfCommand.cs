using Application.Common.Security;
using MediatR;

namespace Application.Users.Commands.Update.Self
{
    [Authorize(Policy = "RequireAuthorization")]
    public class UpdateSelfCommand : IRequest
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}