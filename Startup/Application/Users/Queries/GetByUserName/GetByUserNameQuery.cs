using Application.Common.Security;
using MediatR;

namespace Application.Users.Queries.GetByUserName
{
    [Authorize(Policy = "RequireAuthorization")]
    public class GetByUserNameQuery : IRequest<GetByUserNameViewModel>
    {
    }
}