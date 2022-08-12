using Application.Common.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using Domain.Entities.Identity;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Messages.Commands.Insert
{
    public class InsertMessageCommandHandler : IRequestHandler<InsertMessageCommand, long>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IManagersService _managersService;
        private readonly IMediator _mediator;

        public InsertMessageCommandHandler(IApplicationDbContext context,
                                           ICurrentUserService currentUserService,
                                           IManagersService managersService,
                                           IMediator mediator)
        {
            _context = context;
            _currentUserService = currentUserService;
            _managersService = managersService;
            _mediator = mediator;
        }

        public async Task<long> Handle(InsertMessageCommand request, CancellationToken cancellationToken)
        {
            string errorMessage = null;

            if (!request.IsValid(out errorMessage))
                throw new BadRequestException(errorMessage);

            AppUser sender = await _managersService.FindByUserNameAsync(_currentUserService.UserName);

            if (request.RecipientUserName == sender.UserName)
                throw new BadRequestException(Resources.Translation.MessageYourselfValidation);

            AppUser recipient = await _managersService.FindByUserNameAsync(request.RecipientUserName);

            if (recipient == null)
                throw new NotFoundException(string.Format(Resources.Translation.EntityWasNotFound, nameof(AppUser), request.RecipientUserName));

            Message message = new Message()
            {
                Content = request.Content,
                Sender = sender,
                Recipient = recipient
            };

            await _context.Messages.AddAsync(message);

            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new SendMessageNotification()
            {
                Message = new MessageViewModel()
                {
                    Content = message.Content,
                    DateRead = message.DateRead,
                    Id = message.Id,
                    MessageSent = message.MessageSent,
                    RecipientId = message.Recipient.Id,
                    RecipientPhotoUrl = null,
                    RecipientUserName = message.Recipient.UserName,
                    SenderId = message.Sender.Id,
                    SenderPhotoUrl = null,
                    SenderUserName = message.Sender.UserName,
                }
            }, cancellationToken);

            return message.Id;
        }
    }
}