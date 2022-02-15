using Application.Common.Security;
using MediatR;

namespace Application.Auth.Queries.GetUserRoles
{
    [Authorize(Policy = "RequireAuthorization")]
    public class GetUserRolesQuery : IRequest<string[]>
    {
    }
}