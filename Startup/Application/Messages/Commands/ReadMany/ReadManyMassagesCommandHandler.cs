using Application.Common.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Messages.Commands.ReadMany
{
    public class ReadManyMassagesCommandHandler : IRequestHandler<ReadManyMassagesCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public ReadManyMassagesCommandHandler(IApplicationDbContext context,
                                              ICurrentUserService currentUserService,
                                              IDateTimeService dateTimeService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }

        public async Task<bool> Handle(ReadManyMassagesCommand request, CancellationToken cancellationToken)
        {
            string errorMessage = null;

            if (!request.IsValid(out errorMessage))
                throw new BadRequestException(errorMessage);

            List<Message> unreadMessages = await _context.Messages
                                                         .Where(x => x.RecipientId == _currentUserService.UserId &&
                                                                     x.SenderId == request.RecipientUserId)
                                                         .Where(x => x.DateRead == null)
                                                         .ToListAsync(cancellationToken);

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = _dateTimeService.Now;
                }
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}