﻿using Application.Common.Interfaces;
using Application.Exceptions;
using Domain.Entities.Identity;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.GetUserRoles
{
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, string[]>
    {
        private readonly IManagersService _managersService;
        private readonly ICurrentUserService _currentUserService;

        public GetUserRolesQueryHandler(IManagersService managersService,
                                        ICurrentUserService currentUserService)
        {
            _managersService = managersService;
            _currentUserService = currentUserService;
        }

        public async Task<string[]> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            string[] roles = null;

            AppUser user = await _managersService.FindByUserNameAsync(_currentUserService.UserName);

            if (user == null)
                throw new NotFoundException(string.Format(Resources.Translation.EntityWasNotFound, nameof(AppUser), _currentUserService.UserName));

            roles = await _managersService.GetRoleAsync(user);

            return roles;
        }
    }
}