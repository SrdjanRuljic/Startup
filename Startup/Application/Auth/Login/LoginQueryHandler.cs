using Application.Common.Interfaces;
using Application.Exceptions;
using Domain.Entities.Identity;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, object>
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly IManagersService _managersService;

        public LoginQueryHandler(IJwtFactory jwtFactory,
                                 IManagersService managersService)
        {
            _jwtFactory = jwtFactory;
            _managersService = managersService;
        }

        public async Task<object> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            string errorMessage = null;

            if (!request.IsValid(out errorMessage))
                throw new BadRequestException(errorMessage);

            AppUser user = await _managersService.AuthenticateAsync(request.Username, request.Password);

            if (user == null)
                throw new BadRequestException(ErrorMessages.IncorrectUsernameOrPassword);

            string[] roles = await _managersService.GetRoleAsync(user);

            object token = TokenHelper.GenerateJwt(user.UserName, roles, _jwtFactory);

            return token;
        }
    }
}