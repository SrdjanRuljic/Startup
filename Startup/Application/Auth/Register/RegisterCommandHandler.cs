using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Exceptions;
using Domain.Entities.Identity;
using Domain.Exceptions;
using MediatR;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, object>
    {
        private readonly IManagersService _managersService;
        private readonly IJwtFactory _jwtFactory;

        public RegisterCommandHandler(IManagersService managersService,
                                      IJwtFactory jwtFactory)
        {
            _managersService = managersService;
            _jwtFactory = jwtFactory;
        }

        public async Task<object> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            string errorMessage = null;

            if (!request.IsValid(out errorMessage))
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, errorMessage);

            request.Username = request.Username.ToLower();

            if (String.IsNullOrEmpty(request.FirstName) || String.IsNullOrWhiteSpace(request.FirstName))
                request.FirstName = null;

            if (String.IsNullOrEmpty(request.LastName) || String.IsNullOrWhiteSpace(request.LastName))
                request.LastName = null;

            bool userExist = await _managersService.UserExist(request.Username, request.Email);

            if (userExist)
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, ErrorMessages.UserExists);

            AppUser user = new AppUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Username,
                Email = request.Email
            };

            Result result = await _managersService.CreateUserAsync(user, request.Password, Domain.Enums.Roles.Basic.ToString());

            if (!result.Succeeded)
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, string.Concat(result.Errors));

            string[] roles = await _managersService.GetRoleAsync(user);

            object token = TokenHelper.GenerateJwt(user.UserName, roles, _jwtFactory);

            return token;
        }
    }
}
