using Application.Common.Interfaces;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Identity;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Messages.Commands.Insert
{
    public class InsertMessageCommandHandler : IRequestHandler<InsertMessageCommand, MessageViewModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IManagersService _managersService;
        private readonly IMapper _mapper;

        public InsertMessageCommandHandler(IApplicationDbContext context,
                                           ICurrentUserService currentUserService,
                                           IManagersService managersService,
                                           IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _managersService = managersService;
            _mapper = mapper;
        }

        public async Task<MessageViewModel> Handle(InsertMessageCommand request, CancellationToken cancellationToken)
        {
            string errorMessage = null;

            if (!request.IsValid(out errorMessage))
                throw new BadRequestException(errorMessage);

            AppUser sender = await _managersService.FindByIdAsync(_currentUserService.UserId);

            if (request.RecipientUserId == sender.Id)
                throw new BadRequestException(Resources.Translation.MessageYourselfValidation);

            AppUser recipient = await _managersService.FindByIdAsync(request.RecipientUserId);

            if (recipient == null)
                throw new NotFoundException(string.Format(Resources.Translation.EntityWasNotFound, nameof(AppUser), request.RecipientUserId));

            Message message = new Message()
            {
                Content = request.Content,
                SenderId = sender.Id,
                RecipientId = recipient.Id,
            };

            await _context.Messages.AddAsync(message, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            message.Sender = sender;
            message.Recipient = recipient;

            return _mapper.Map<MessageViewModel>(message);
        }
    }
}