using Application.Common.Pagination.Models;
using Application.Common.Security;
using MediatR;

namespace Application.Messages.Queries.Search
{
    [Authorize(Policy = "RequireAuthorization")]
    public class SearchMessagesQuery : PaginationViewModel, IRequest<PaginationResultViewModel<SearchMessagesQueryViewModel>>
    {
        public string Container { get; set; } = "Unread";
    }
}