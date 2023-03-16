using Application.Common.Security;
using MediatR;

namespace Application.Messages.Commands.ReadMany
{
    [Authorize(Policy = "RequireAuthorization")]
    public class ReadManyMassagesCommand : IRequest<bool>
    {
        public string RecipientUserId { get; set; }
    }
}