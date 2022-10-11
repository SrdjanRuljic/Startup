using Application.Common.Interfaces;
using Application.Exceptions;
using Domain.Entities.Identity;
using Domain.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Commands.Refresh
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, object>
    {
        private readonly IApplicationDbContext _context;
        private readonly IJwtFactory _jwtFactory;
        private readonly IManagersService _managersService;

        public RefreshTokenCommandHandler(IApplicationDbContext context, IJwtFactory jwtFactory, IManagersService managersService)
        {
            _context = context;
            _jwtFactory = jwtFactory;
            _managersService = managersService;
        }

        public async Task<object> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            bool isValid = _jwtFactory.Validate(request.RefreshToken);

            if (!isValid)
                throw new BadRequestException(Resources.Translation.InvalidRefreshToken);

            RefreshToken refreshToken = await _context.RefreshTokens
                                                      .Where(x => x.Token == request.RefreshToken)
                                                      .FirstOrDefaultAsync();

            if (refreshToken == null)
                throw new NotFoundException(string.Format(Resources.Translation.EntityWasNotFound, nameof(RefreshToken), request.RefreshToken));

            _context.RefreshTokens.Remove(refreshToken);

            AppUser user = await _managersService.FindByIdAsync(refreshToken.UserId);

            if (user == null)
                throw new NotFoundException(string.Format(Resources.Translation.EntityWasNotFound, nameof(AppUser), refreshToken.UserId));

            string[] roles = await _managersService.GetRolesAsync(user);

            object tokens = TokenHelper.GenerateJwt(user.Id, roles, _jwtFactory);

            string refershToken = tokens?.GetType()
                                         .GetProperty("refresh_token")?
                                         .GetValue(tokens, null)
                                         .ToString();

            await _context.RefreshTokens.AddAsync(new RefreshToken(user.Id, refershToken));

            await _context.SaveChangesAsync(cancellationToken);

            return tokens;
        }
    }
}