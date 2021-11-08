using Application.Common.Pagination.Models;
using MediatR;

namespace Application.Users.Queries.Search
{
    public class SearchUsersQuery : PaginationViewModel, IRequest<PaginationResultViewModel<SearchUsersViewModel>>
    {
    }
}
