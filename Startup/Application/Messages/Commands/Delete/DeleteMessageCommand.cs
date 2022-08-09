using Application.Common.Security;
using MediatR;

namespace Application.Messages.Commands.Delete
{
    [Authorize(Policy = "RequireAuthorization")]
    public class DeleteMessageCommand : IRequest
    {
        public long Id { get; set; }
    }
}