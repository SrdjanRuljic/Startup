using Application.Common.Security;
using Domain.Entities;
using MediatR;

namespace Application.Groups.Queries.GetForConnection
{
    [Authorize(Policy = "RequireAuthorization")]
    public class GetForConnectionQuery : IRequest<Group>
    {
        public string ConnectionId { get; set; }
    }
}