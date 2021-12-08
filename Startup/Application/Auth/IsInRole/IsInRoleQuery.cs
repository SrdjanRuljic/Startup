using MediatR;

namespace Application.Auth.IsInRole
{
    public class IsInRoleQuery : IRequest<bool>
    {
        public string Role { get; set; }
    }
}
