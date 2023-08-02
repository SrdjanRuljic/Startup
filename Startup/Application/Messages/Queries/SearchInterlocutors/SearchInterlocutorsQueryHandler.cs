using Application.Common.Interfaces;
using Application.Common.Pagination.Models;
using Application.Common.Pagination;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace Application.Messages.Queries.SearchInterlocutors
{
    public class SearchInterlocutorsQueryHandler : IRequestHandler<SearchInterlocutorsQuery, PaginationResultViewModel<SearchInterlocutorsViewModel>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IManagersService _managersService;
        private readonly IMapper _mapper;

        public SearchInterlocutorsQueryHandler(IApplicationDbContext context,
                                               ICurrentUserService currentUserService,
                                               IManagersService managersService,
                                               IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _managersService = managersService;
            _mapper = mapper;
        }

        public async Task<PaginationResultViewModel<SearchInterlocutorsViewModel>> Handle(SearchInterlocutorsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<SearchInterlocutorsViewModel> query = string.IsNullOrEmpty(request.Term) ?
                                                             _context.Messages
                                                                     .Where(x => x.RecipientId == _currentUserService.UserId ||
                                                                                 x.SenderId == _currentUserService.UserId)
                                                                     .Select(x => new SearchInterlocutorsViewModel()
                                                                     {
                                                                         Id = x.SenderId == _currentUserService.UserId ? x.Recipient.Id : x.Sender.Id,
                                                                         UserName = x.SenderId == _currentUserService.UserId ? x.Recipient.UserName : x.Sender.UserName,
                                                                         FirstName = x.SenderId == _currentUserService.UserId ? x.Recipient.FirstName : x.Sender.FirstName,
                                                                         LastName = x.SenderId == _currentUserService.UserId ? x.Recipient.LastName : x.Sender.LastName,
                                                                     }).Distinct() :
                                                             _managersService.GetUsers()
                                                                             .Where(x => x.Id != _currentUserService.UserId)
                                                                             .Where(x => x.UserName.Contains(request.Term) ||
                                                                                         x.FirstName.Contains(request.Term) ||
                                                                                         x.LastName.Contains(request.Term))
                                                                             .ProjectTo<SearchInterlocutorsViewModel>(_mapper.ConfigurationProvider)
                                                                             .Distinct();

            PaginatedList<SearchInterlocutorsViewModel> paginatedList = await PaginatedList<SearchInterlocutorsViewModel>.CreateAsync(query,
                                                                                                                                      request.PageNumber,
                                                                                                                                      request.PageSize,
                                                                                                                                      cancellationToken);

            return new PaginationResultViewModel<SearchInterlocutorsViewModel>
            {
                List = paginatedList,
                PageNumber = paginatedList.PageNumber,
                TotalPages = paginatedList.TotalPages,
                TotalCount = paginatedList.TotalCount
            };
        }
    }
}