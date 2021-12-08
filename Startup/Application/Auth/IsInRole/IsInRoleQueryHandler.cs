using Application.Common.Interfaces;
using Domain.Entities.Identity;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.IsInRole
{
    public class IsInRoleQueryHandler : IRequestHandler<IsInRoleQuery, bool>
    {
        private readonly IManagersService _managersService;
        private readonly ICurrentUserService _currentUserService;

        public IsInRoleQueryHandler(IManagersService managersService,
                                    ICurrentUserService currentUserService)
        {
            _managersService = managersService;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(IsInRoleQuery request, CancellationToken cancellationToken)
        {
            bool isInRole = false;

            AppUser user = await _managersService.FindByUserNameAsync(_currentUserService.UserName);

            if (user != null)
                isInRole = await _managersService.IsUserInRoleAsync(user, request.Role);

            return isInRole;
        }
    }
}
