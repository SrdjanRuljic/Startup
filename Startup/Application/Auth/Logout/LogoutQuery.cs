using Application.Common.Security;
using MediatR;

namespace Application.Auth.Logout
{
    [Authorize(Policy = "RequireAuthorization")]
    public class LogoutQuery : IRequest
    {
    }
}
