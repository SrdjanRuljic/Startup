using Application.Auth.Commands.Register.Notification;
using Application.Common.Behaviours;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Exceptions;
using Domain.Entities.Identity;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
    {
        private readonly IManagersService _managersService;
        private readonly IMediator _mediator;

        public RegisterCommandHandler(IManagersService managersService, IMediator mediator)
        {
            _managersService = managersService;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            string errorMessage = null;

            if (!request.IsValid(out errorMessage))
                throw new BadRequestException(errorMessage);

            request.Username = request.Username.ToLower();

            if (String.IsNullOrEmpty(request.FirstName) || String.IsNullOrWhiteSpace(request.FirstName))
                request.FirstName = null;

            if (String.IsNullOrEmpty(request.LastName) || String.IsNullOrWhiteSpace(request.LastName))
                request.LastName = null;

            AppUser user = new AppUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Username,
                Email = request.Email
            };

            Result result = await _managersService.CreateUserAsync(user, request.Password, new string[] { Domain.Enums.Roles.Basic.ToString() });

            if (!result.Succeeded)
                throw new BadRequestException(string.Concat(result.Errors));

            string token = await _managersService.GenerateEmailConfirmationTokenAsync(user);
            string link = LinkMaker.CreateConfirmLink(user.UserName, request.ClientUri, token);

            Message message = new Message(new string[] { user.Email }, "Email confirmation", link, null);

            await _mediator.Publish(new SendConfirmationEmailNotification()
            {
                Email = user.Email,
                Link = link,
            });

            return Unit.Value;
        }
    }
}