using Application.Common.Interfaces;
using Application.Exceptions;
using MediatR;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetDisplayName
{
    public class GetDisplayNameQueryHandler : IRequestHandler<GetDisplayNameQuery, string>
    {
        private readonly IManagersService _managersService;
        private readonly ICurrentUserService _currentUserService;

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
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, ErrorMessages.DataNotFound);

            return displayName;
        }
    }
}