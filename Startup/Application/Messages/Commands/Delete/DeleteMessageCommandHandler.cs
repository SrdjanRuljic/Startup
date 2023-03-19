using Application.Common.Interfaces;
using Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Messages.Commands.Delete
{
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteMessageCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.Message message = await _context.Messages
                                                            .Include(x => x.Sender)
                                                            .Include(x => x.Recipient)
                                                            .Where(x => x.Id == request.Id)
                                                            .FirstOrDefaultAsync(cancellationToken);

            if (message == null)
                throw new NotFoundException(string.Format(Resources.Translation.EntityWasNotFound, nameof(Domain.Entities.Message), request.Id));

            if (message.SenderId != _currentUserService.UserId && message.RecipientId != _currentUserService.UserId)
                throw new UnauthorizedAccessException(ErrorMessages.Unauthorised);

            if (message.SenderId == _currentUserService.UserId)
                message.SenderDeleted = true;

            if (message.RecipientId == _currentUserService.UserId)
                message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
                _context.Messages.Remove(message);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}