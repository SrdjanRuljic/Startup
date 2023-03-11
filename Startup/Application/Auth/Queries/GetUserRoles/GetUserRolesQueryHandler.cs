﻿using Application.Common.Interfaces;
using Application.Exceptions;
using Domain.Entities.Identity;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Queries.GetUserRoles
{
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, string[]>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IManagersService _managersService;

        public GetUserRolesQueryHandler(IManagersService managersService,
                                        ICurrentUserService currentUserService)
        {
            _managersService = managersService;
            _currentUserService = currentUserService;
        }

        public async Task<string[]> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            string[] roles = null;

            AppUser user = await _managersService.FindByIdAsync(_currentUserService.UserId);

            if (user == null)
                throw new NotFoundException(string.Format(Resources.Translation.EntityWasNotFound, nameof(AppUser), _currentUserService.UserId));

            roles = await _managersService.GetRolesAsync(user);

            return roles;
        }
    }
}