using Application.Common.Security;
using MediatR;

namespace Application.Connections.Commands.Delete
{
    [Authorize(Policy = "RequireAuthorization")]
    public class DeleteConnectionCommand : IRequest
    {
        public string Id { get; set; }
    }
}