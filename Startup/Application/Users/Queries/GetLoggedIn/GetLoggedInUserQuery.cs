using Application.Common.Security;
using MediatR;

namespace Application.Users.Queries.GetLoggedIn
{
    [Authorize(Policy = "RequireAuthorization")]
    public class GetLoggedInUserQuery : IRequest<GetLoggedInUserViewModel>
    {
    }
}