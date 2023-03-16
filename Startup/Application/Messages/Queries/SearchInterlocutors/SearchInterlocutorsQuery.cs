using Application.Common.Pagination.Models;
using Application.Common.Security;
using MediatR;

namespace Application.Messages.Queries.SearchInterlocutors
{
    [Authorize(Policy = "RequireAuthorization")]
    public class SearchInterlocutorsQuery : PaginationViewModel, IRequest<PaginationResultViewModel<SearchInterlocutorsViewModel>>
    {
        public string Term { get; set; }
    }
}