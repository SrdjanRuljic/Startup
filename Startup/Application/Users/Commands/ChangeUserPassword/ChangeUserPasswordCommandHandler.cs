using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Exceptions;
using Domain.Entities.Identity;
using MediatR;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.ChangeUserPassword
{
    public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand>
    {
        private readonly IManagersService _managersService;

        public ChangeUserPasswordCommandHandler(IManagersService managersService)
        {
            _managersService = managersService;
        }

        public async Task<Unit> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            string errorMessage = null;

            if (!request.IsValid(out errorMessage))
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, errorMessage);

            AppUser user = await _managersService.FindByIdAsync(request.Id);

            if (user == null)   
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, ErrorMessages.DataNotFound);

            string token = await _managersService.GenerateResetPasswordTokenAsync(user);

            Result result = await _managersService.ResetPasswordAsync(user, token, request.NewPassword);

            if (!result.Succeeded)
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, string.Concat(result.Errors));

            return Unit.Value;
        }
    }
}
