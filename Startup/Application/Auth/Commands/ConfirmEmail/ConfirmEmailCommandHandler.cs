﻿using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Exceptions;
using Domain.Entities.Identity;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand>
    {
        private readonly IManagersService _managersService;

        public ConfirmEmailCommandHandler(IManagersService managersService)
        {
            _managersService = managersService;
        }

        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            string errorMessage = null;

            if (!request.IsValid(out errorMessage))
                throw new BadRequestException(errorMessage);

            AppUser user = await _managersService.FindByUserNameAsync(request.UserName);

            if (user == null)
                throw new NotFoundException(string.Format(Resources.Translation.EntityWasNotFound, nameof(AppUser), request.UserName));

            Result result = await _managersService.ConfirmEmailAsync(user, request.Token);

            if (!result.Succeeded)
                throw new BadRequestException(string.Concat(result.Errors));

            return Unit.Value;
        }
    }
}