using Application.Common.Security;
using Domain.Entities;
using MediatR;

namespace Application.Connections.Commands.Delete
{
    [Authorize(Policy = "RequireAuthorization")]
    public class DeleteConnectionCommand : IRequest<Group>
    {
        public string Id { get; set; }
    }
}