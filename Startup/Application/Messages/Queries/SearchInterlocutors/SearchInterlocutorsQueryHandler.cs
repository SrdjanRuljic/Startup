using Application.Common.Interfaces;
using Application.Common.Pagination;
using Application.Common.Pagination.Models;
using AutoMapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Messages.Queries.SearchInterlocutors
{
    public class SearchInterlocutorsQueryHandler : IRequestHandler<SearchInterlocutorsQuery, PaginationResultViewModel<SearchInterlocutorsViewModel>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public SearchInterlocutorsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<PaginationResultViewModel<SearchInterlocutorsViewModel>> Handle(SearchInterlocutorsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<SearchInterlocutorsViewModel> list = _context.Messages
                                                                    .Where(x => x.Recipient.UserName == _currentUserService.UserName ||
                                                                                x.Sender.UserName == _currentUserService.UserName)
                                                                    .Select(x => new SearchInterlocutorsViewModel()
                                                                    {
                                                                        Id = x.Sender.UserName == _currentUserService.UserName ? x.Recipient.Id : x.Sender.Id,
                                                                        UserName = x.Sender.UserName == _currentUserService.UserName ? x.Recipient.UserName : x.Sender.UserName,
                                                                        FirstName = x.Sender.UserName == _currentUserService.UserName ? x.Recipient.FirstName : x.Sender.FirstName,
                                                                        LastName = x.Sender.UserName == _currentUserService.UserName ? x.Recipient.LastName : x.Sender.LastName,
                                                                    }).Distinct();

            PaginatedList<SearchInterlocutorsViewModel> paginatedList = await PaginatedList<SearchInterlocutorsViewModel>.CreateAsync(list,
                                                                                                                                      request.PageNumber,
                                                                                                                                      request.PageSize);

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