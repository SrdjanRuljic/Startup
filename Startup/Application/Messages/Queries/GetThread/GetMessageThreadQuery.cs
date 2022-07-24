using Application.Common.Security;
using MediatR;
using System.Collections.Generic;

namespace Application.Messages.Queries.GetThread
{
    [Authorize(Policy = "RequireAuthorization")]
    public class GetMessageThreadQuery : IRequest<IEnumerable<GetMessageThreadQueryViewModel>>
    {
        public string RecipientUserName { get; set; }
    }
}