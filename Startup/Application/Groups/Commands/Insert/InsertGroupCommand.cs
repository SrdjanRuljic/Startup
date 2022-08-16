using Application.Common.Security;
using Domain.Entities;
using MediatR;

namespace Application.Groups.Commands.Insert
{
    [Authorize(Policy = "RequireAuthorization")]
    public class InsertGroupCommand : IRequest<Group>
    {
        public string ConnectionId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
    }
}