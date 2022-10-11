using Application.Common.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using Domain.Entities.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IManagersService _managersService;

        public LogoutCommandHandler(IApplicationDbContext context,
                                    ICurrentUserService currentUserService,
                                    IManagersService managersService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _managersService = managersService;
        }

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await _managersService.SignOutAsync();

            AppUser user = await _managersService.FindByUserNameAsync(_currentUserService.UserName);

            if (user == null)
                throw new NotFoundException(string.Format(Resources.Translation.EntityWasNotFound, nameof(AppUser), _currentUserService.UserName));

            List<RefreshToken> refreshTokens = await _context.RefreshTokens
                                                             .Where(x => x.UserId == user.Id)
                                                             .ToListAsync();

            _context.RefreshTokens.RemoveRange(refreshTokens);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}