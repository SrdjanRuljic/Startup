using MediatR;

namespace Application.Auth.GetUserRoles
{
    public class GetUserRolesQuery : IRequest<string[]>
    {
    }
}