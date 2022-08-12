using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Messages.Queries.GetThread
{
    public class GetMessageThreadQueryHandler : IRequestHandler<GetMessageThreadQuery, IEnumerable<MessageViewModel>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetMessageThreadQueryHandler(IApplicationDbContext context,
                                            ICurrentUserService currentUserService,
                                            IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MessageViewModel>> Handle(GetMessageThreadQuery request, CancellationToken cancellationToken)
        {
            List<MessageViewModel> messages = await _context.Messages
                                                            .Where(x => x.Recipient.UserName == _currentUserService.UserName &&
                                                                        x.RecipientDeleted == false &&
                                                                        x.Sender.UserName == request.RecipientUserName ||
                                                                        x.Recipient.UserName == request.RecipientUserName &&
                                                                        x.Sender.UserName == _currentUserService.UserName &&
                                                                        x.SenderDeleted == false)
                                                            .OrderBy(x => x.MessageSent)
                                                            .ProjectTo<MessageViewModel>(_mapper.ConfigurationProvider)
                                                            .ToListAsync();

            return messages;
        }
    }
}