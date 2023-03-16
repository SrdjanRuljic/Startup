using Application.Common.Security;
using MediatR;
using System.Collections.Generic;

namespace Application.Messages.Queries.GetThread
{
    [Authorize(Policy = "RequireAuthorization")]
    public class GetMessageThreadQuery : IRequest<IEnumerable<MessageViewModel>>
    {
        public string RecipientUserId { get; set; }
    }
}