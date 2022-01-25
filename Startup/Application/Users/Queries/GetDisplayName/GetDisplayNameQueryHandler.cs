using Application.Common.Interfaces;
using Application.Exceptions;
using Domain.Entities.Identity;
using MediatR;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetDisplayName
{
    public class GetDisplayNameQueryHandler : IRequestHandler<GetDisplayNameQuery, string>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IManagersService _managersService;

        public GetDisplayNameQueryHandler(IManagersService managersService,
                                          ICurrentUserService currentUserService)
        {
            _managersService = managersService;
            _currentUserService = currentUserService;
        }

        public async Task<string> Handle(GetDisplayNameQuery request, CancellationToken cancellationToken)
        {
            string displayName = await _managersService.GetDisplayNameAsync(_currentUserService.UserName);

            if (displayName == null)
                throw new NotFoundException(string.Format(Resources.Translation.EntityWasNotFound, nameof(AppUser), _currentUserService.UserName));

            return displayName;
        }
    }
}