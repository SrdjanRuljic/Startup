using Application.Common.Security;
using MediatR;

namespace Application.Messages.Commands.Insert
{
    [Authorize(Policy = "RequireAuthorization")]
    public class InsertMessageCommand : IRequest<long>
    {
        public string Content { get; set; }
        public string RecipientUserName { get; set; }
    }
}