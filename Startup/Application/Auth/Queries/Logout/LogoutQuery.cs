using Application.Common.Security;
using MediatR;

namespace Application.Auth.Queries.Logout
{
    [Authorize(Policy = "RequireAuthorization")]
    public class LogoutQuery : IRequest
    {
    }
}