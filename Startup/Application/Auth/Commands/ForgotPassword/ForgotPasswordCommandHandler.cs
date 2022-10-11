using Application.Auth.Commands.ForgotPassword.Notification;
using Application.Auth.Commands.Register.Notification;
using Application.Common.Behaviours;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Exceptions;
using Domain.Entities.Identity;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IManagersService _managersService;
        private readonly IMediator _mediator;

        public ForgotPasswordCommandHandler(IManagersService managersService, IMediator mediator)
        {
            _managersService = managersService;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            string errorMessage = null;

            if (!request.IsValid(out errorMessage))
                throw new BadRequestException(errorMessage);

            AppUser user = await _managersService.FindByEmailAsync(request.Email);

            if (user == null)
                throw new NotFoundException(string.Format(Resources.Translation.EntityWasNotFound, nameof(AppUser), request.Email));

            string token = await _managersService.GenerateResetPasswordTokenAsync(user);
            string link = LinkMaker.CreateResetPasswordLink(user.Email, request.ClientUri, token);

            await _mediator.Publish(new SendResetPasswordEmailNotification()
            {
                Email = user.Email,
                Link = link,
            });

            return Unit.Value;
        }
    }
}