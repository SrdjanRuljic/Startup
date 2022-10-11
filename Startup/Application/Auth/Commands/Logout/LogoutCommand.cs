using Application.Common.Security;
using MediatR;

namespace Application.Auth.Commands.Logout
{
    [Authorize(Policy = "RequireAuthorization")]
    public class LogoutCommand : IRequest
    {
    }
}