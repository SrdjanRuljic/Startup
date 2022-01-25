using Application.Common.Behaviours;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Exceptions;
using Domain.Entities.Identity;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
    {
        private readonly IEmailSenderService _emailSenderService;
        private readonly IManagersService _managersService;

        public RegisterCommandHandler(IEmailSenderService emailSenderService,
                                      IManagersService managersService)
        {
            _emailSenderService = emailSenderService;
            _managersService = managersService;
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

            await _emailSenderService.SendConfirmationEmailAsync(message);

            return Unit.Value;
        }
    }
}