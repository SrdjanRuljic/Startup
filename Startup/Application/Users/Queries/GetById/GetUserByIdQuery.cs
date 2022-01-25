using Application.Common.Security;
using MediatR;

namespace Application.Users.Queries.GetById
{
    [Authorize(Policy = "RequireAuthorization")]
    public class GetUserByIdQuery : IRequest<GetUserByIdViewModel>
    {
        public string Id { get; set; }
    }
}