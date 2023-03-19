using Application.Common.Interfaces;
using Application.Common.Pagination.Models;
using Application.Common.Pagination;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Messages.Queries.Search
{
    public class SearchMessagesQueryHandler : IRequestHandler<SearchMessagesQuery, PaginationResultViewModel<SearchMessagesQueryViewModel>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public SearchMessagesQueryHandler(IApplicationDbContext context,
                                          ICurrentUserService currentUserService,
                                          IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<PaginationResultViewModel<SearchMessagesQueryViewModel>> Handle(SearchMessagesQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Message> query = _context.Messages.OrderByDescending(x => x.MessageSent).AsQueryable();

            query = request.Container switch
            {
                "Inbox" => query.Where(x => x.RecipientId == _currentUserService.UserId &&
                                            x.RecipientDeleted == false),
                "Outbox" => query.Where(x => x.SenderId == _currentUserService.UserId &&
                                             x.SenderDeleted == false),
                _ => query.Where(x => x.RecipientId == _currentUserService.UserId &&
                                      x.RecipientDeleted == false &&
                                      x.DateRead == null)
            };

            IQueryable<SearchMessagesQueryViewModel> list = query.ProjectTo<SearchMessagesQueryViewModel>(_mapper.ConfigurationProvider);

            PaginatedList<SearchMessagesQueryViewModel> paginatedList = await PaginatedList<SearchMessagesQueryViewModel>.CreateAsync(list,
                                                                                                                                      request.PageNumber,
                                                                                                                                      request.PageSize,
                                                                                                                                      cancellationToken);

            return new PaginationResultViewModel<SearchMessagesQueryViewModel>
            {
                List = paginatedList,
                PageNumber = paginatedList.PageNumber,
                TotalPages = paginatedList.TotalPages,
                TotalCount = paginatedList.TotalCount
            };
        }
    }
}