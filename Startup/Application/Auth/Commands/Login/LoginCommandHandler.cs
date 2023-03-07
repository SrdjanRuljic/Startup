using Application.Common.Interfaces;
using Application.Exceptions;
using Domain.Entities.Identity;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, object>
    {
        private readonly IApplicationDbContext _context;
        private readonly IJwtFactory _jwtFactory;
        private readonly IManagersService _managersService;

        public LoginCommandHandler(IApplicationDbContext context,
                                 IJwtFactory jwtFactory,
                                 IManagersService managersService)
        {
            _context = context;
            _jwtFactory = jwtFactory;
            _managersService = managersService;
        }

        public async Task<object> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            string errorMessage = null;

            if (!request.IsValid(out errorMessage))
                throw new BadRequestException(errorMessage);

            AppUser user = await _managersService.AuthenticateAsync(request.Username, request.Password);

            if (user == null)
                throw new BadRequestException(ErrorMessages.IncorrectUsernameOrPassword);

            string[] roles = await _managersService.GetRolesAsync(user);

            object tokens = TokenHelper.GenerateJwt(user.UserName, roles, _jwtFactory);

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