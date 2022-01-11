using Application.Common.Interfaces;
using Application.Exceptions;
using Domain.Entities.Identity;
using Domain.Exceptions;
using MediatR;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, object>
    {
        private readonly IManagersService _managersService;
        private readonly IJwtFactory _jwtFactory;

        public LoginQueryHandler(IManagersService managersService,
                                 IJwtFactory jwtFactory)
        {
            _managersService = managersService;
            _jwtFactory = jwtFactory;
        }

        public async Task<object> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            string errorMessage = null;

            if (!request.IsValid(out errorMessage))
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, errorMessage);

            AppUser user = await _managersService.AuthenticateAsync(request.Username, request.Password);

            if (user == null)
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, ErrorMessages.IncorrectUsernameOrPassword);

            string[] roles = await _managersService.GetRoleAsync(user);

            object token = TokenHelper.GenerateJwt(user.UserName, roles, _jwtFactory);

            return token;
        }
    }
}