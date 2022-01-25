using Application.Common.Interfaces;
using Application.Common.Security;
using Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviours
{
    public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IManagersService _managersService;

        public AuthorizationBehaviour(ICurrentUserService currentUserService, 
                                      IManagersService managersService)
        {
            _currentUserService = currentUserService;
            _managersService = managersService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            IEnumerable<AuthorizeAttribute> authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

            if (authorizeAttributes.Any())
            {
                if (_currentUserService.UserName == null)
                    throw new UnauthorizedAccessException(ErrorMessages.Unauthorised);

                IEnumerable<AuthorizeAttribute> authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));

                if (authorizeAttributesWithRoles.Any())
                {
                    bool authorized = false;

                    foreach (string[] roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(',')))
                    {
                        foreach (string role in roles)
                        {
                            bool isInRole = await _managersService.IsInRoleAsync(_currentUserService.UserName, role.Trim());
                            if (isInRole)
                            {
                                authorized = true;
                                break;
                            }
                        }
                    }

                    if (!authorized)
                        throw new ForbiddenAccessException(ErrorMessages.Forbidden);
                }

                IEnumerable<AuthorizeAttribute> authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));

                if (authorizeAttributesWithPolicies.Any())
                {
                    foreach (string policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
                    {
                        bool authorized = await _managersService.AuthorizeAsync(_currentUserService.UserName, policy);

                        if (!authorized)
                        {
                            throw new ForbiddenAccessException(ErrorMessages.Forbidden);
                        }
                    }
                }
            }

            return await next();
        }
    }
}