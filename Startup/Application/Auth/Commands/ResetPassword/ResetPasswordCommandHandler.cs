using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Exceptions;
using Domain.Entities.Identity;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IManagersService _managersService;

        public ResetPasswordCommandHandler(IManagersService managersService)
        {
            _managersService = managersService;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            string errorMessage = null;

            if (!request.IsValid(out errorMessage))
                throw new BadRequestException(errorMessage);

            AppUser user = await _managersService.FindByEmailAsync(request.Email);

            if (user == null)
                throw new NotFoundException(string.Format(Resources.Translation.EntityWasNotFound, nameof(AppUser), request.Email));

            Result result = await _managersService.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
                throw new BadRequestException(string.Concat(result.Errors));

            return Unit.Value;
        }
    }
}