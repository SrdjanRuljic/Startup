using Application.Common.Security;
using MediatR;

namespace Application.Users.Queries.GetDisplayName
{
    [Authorize(Policy = "RequireAuthorization")]
    public class GetDisplayNameQuery : IRequest<string>
    {
    }
}