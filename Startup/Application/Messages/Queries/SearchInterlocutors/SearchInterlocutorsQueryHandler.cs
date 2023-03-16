using Application.Common.Interfaces;
using Application.Common.Pagination.Models;
using Application.Common.Pagination;
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

        public SearchInterlocutorsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<PaginationResultViewModel<SearchInterlocutorsViewModel>> Handle(SearchInterlocutorsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<SearchInterlocutorsViewModel> list = _context.Messages
                                                                    .Where(x => x.RecipientId == _currentUserService.UserId ||
                                                                                x.SenderId == _currentUserService.UserId)
                                                                    .Select(x => new SearchInterlocutorsViewModel()
                                                                    {
                                                                        Id = x.SenderId == _currentUserService.UserId ? x.Recipient.Id : x.Sender.Id,
                                                                        UserName = x.SenderId == _currentUserService.UserId ? x.Recipient.UserName : x.Sender.UserName,
                                                                        FirstName = x.SenderId == _currentUserService.UserId ? x.Recipient.FirstName : x.Sender.FirstName,
                                                                        LastName = x.SenderId == _currentUserService.UserId ? x.Recipient.LastName : x.Sender.LastName,
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