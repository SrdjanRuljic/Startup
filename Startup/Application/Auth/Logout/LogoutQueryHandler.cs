using Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Logout
{
    public class LogoutQueryHandler : IRequestHandler<LogoutQuery>
    {
        private readonly IManagersService _managersService;

        public LogoutQueryHandler(IManagersService managersService)
        {
            _managersService = managersService;
        }

        public async Task<Unit> Handle(LogoutQuery request, CancellationToken cancellationToken)
        {
            await _managersService.SignOutAsync();

            return Unit.Value;
        }
    }
}