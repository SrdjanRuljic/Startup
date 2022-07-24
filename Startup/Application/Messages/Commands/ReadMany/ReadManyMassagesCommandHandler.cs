using Application.Common.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
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

        public ReadManyMassagesCommandHandler(IApplicationDbContext context,
                                              ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(ReadManyMassagesCommand request, CancellationToken cancellationToken)
        {
            string errorMessage = null;

            if (!request.IsValid(out errorMessage))
                throw new BadRequestException(errorMessage);

            List<Message> unreadMessages = await _context.Messages
                                                         .Where(x => x.Recipient.UserName == _currentUserService.UserName &&
                                                                     x.Sender.UserName == request.RecipientUserName ||
                                                                     x.Recipient.UserName == request.RecipientUserName &&
                                                                     x.Sender.UserName == _currentUserService.UserName)
                                                         .Where(x => x.DateRead == null)
                                                         .ToListAsync();

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}