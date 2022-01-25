using Application.Common.Behaviours;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Exceptions;
using Domain.Entities.Identity;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.Insert
{
    public class InsertUserCommandHandler : IRequestHandler<InsertUserCommand>
    {
        private readonly IEmailSenderService _emailSenderService;
        private readonly IManagersService _managersService;

        public InsertUserCommandHandler(IManagersService managersService,
                                        IEmailSenderService emailSenderService)
        {
            _managersService = managersService;
            _emailSenderService = emailSenderService;
        }

        public async Task<Unit> Handle(InsertUserCommand request, CancellationToken cancellationToken)
        {
            string errorMessage = null;

            if (!request.IsValid(out errorMessage))
                throw new BadRequestException(errorMessage);

            request.UserName = request.UserName.ToLower();

            if (String.IsNullOrEmpty(request.FirstName) || String.IsNullOrWhiteSpace(request.FirstName))
                request.FirstName = null;

            if (String.IsNullOrEmpty(request.LastName) || String.IsNullOrWhiteSpace(request.LastName))
                request.LastName = null;

            bool userExist = await _managersService.UserExistAsync(request.UserName, request.Email);

            if (userExist)
                throw new BadRequestException(ErrorMessages.UserExists);

            AppUser user = new AppUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                Email = request.Email,
                EmailConfirmed = true
            };

            string password = PasswordGenerator.GeneratePasword();

            Result result = await _managersService.CreateUserAsync(user, password, request.Roles);

            if (!result.Succeeded)
                throw new BadRequestException(string.Concat(result.Errors));

            Message message = new Message(new string[] { user.Email }, "Password send", null, password);

            await _emailSenderService.SendPasswordEmailAsync(message);

            return Unit.Value;
        }
    }
}