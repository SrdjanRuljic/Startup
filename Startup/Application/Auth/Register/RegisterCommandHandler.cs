﻿using Application.Common.Behaviours;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities.Identity;
using Domain.Exceptions;
using MediatR;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
    {
        private readonly IManagersService _managersService;
        private readonly IJwtFactory _jwtFactory;
        private readonly IEmailSenderService _emailSenderService;

        public RegisterCommandHandler(IManagersService managersService,
                                      IJwtFactory jwtFactory,
                                      IEmailSenderService emailSenderService)
        {
            _managersService = managersService;
            _jwtFactory = jwtFactory;
            _emailSenderService = emailSenderService;
        }

        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            string errorMessage = null;

            if (!request.IsValid(out errorMessage))
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, errorMessage);

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
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, string.Concat(result.Errors));

            string token = await _managersService.GenerateEmailConfirmationTokenAsync(user);
            string link = LinkMaker.CreateConfirmLink(user.UserName, request.ClientUri, token);

            Message message = new Message(new string[] { user.Email }, "Email confirmation", link, null);

            await _emailSenderService.SendConfirmationEmailAsync(message);

            return Unit.Value;
        }
    }
}
