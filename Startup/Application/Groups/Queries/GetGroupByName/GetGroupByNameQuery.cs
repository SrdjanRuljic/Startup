using Application.Common.Security;
using Domain.Entities;
using MediatR;

namespace Application.Groups.Queries.GetGroupByName
{
    [Authorize(Policy = "RequireAuthorization")]
    public class GetGroupByNameQuery : IRequest<Group>
    {
        public string Name { get; set; }
    }
}