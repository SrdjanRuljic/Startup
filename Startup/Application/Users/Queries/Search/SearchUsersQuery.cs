using Application.Common.Pagination.Models;
using Application.Common.Security;
using MediatR;

namespace Application.Users.Queries.Search
{
    [Authorize(Policy = "RequireAdminRole")]
    public class SearchUsersQuery : PaginationViewModel, IRequest<PaginationResultViewModel<SearchUsersViewModel>>
    {
        public string Term { get; set; }
    }
}