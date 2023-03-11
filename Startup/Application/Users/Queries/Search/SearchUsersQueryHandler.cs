using Application.Common.Interfaces;
using Application.Common.Pagination;
using Application.Common.Pagination.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.Search
{
    public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, PaginationResultViewModel<SearchUsersViewModel>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IManagersService _managersService;
        private readonly IMapper _mapper;

        public SearchUsersQueryHandler(ICurrentUserService currentUserService, IManagersService managersService, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _managersService = managersService;
            _mapper = mapper;
        }

        public async Task<PaginationResultViewModel<SearchUsersViewModel>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
        {
            IQueryable<SearchUsersViewModel> list = _managersService.GetUsers()
                                                                    .Where(x => x.UserName != _currentUserService.UserName)
                                                                    .Where(x => string.IsNullOrEmpty(request.Term) ?
                                                                                true :
                                                                                x.FirstName.Contains(request.Term) ||
                                                                                x.LastName.Contains(request.Term) ||
                                                                                x.UserName.Contains(request.Term))
                                                                    .ProjectTo<SearchUsersViewModel>(_mapper.ConfigurationProvider)
                                                                    .OrderBy(x => x.UserName);

            PaginatedList<SearchUsersViewModel> paginatedList = await PaginatedList<SearchUsersViewModel>.CreateAsync(list,
                                                                                                                      request.PageNumber,
                                                                                                                      request.PageSize);

            return new PaginationResultViewModel<SearchUsersViewModel>
            {
                List = paginatedList,
                PageNumber = paginatedList.PageNumber,
                TotalPages = paginatedList.TotalPages,
                TotalCount = paginatedList.TotalCount
            };
        }
    }
}