using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Exceptions;
using Domain.Entities.Identity;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IManagersService _managersService;

        public ChangePasswordCommandHandler(IManagersService managersService,
                                            ICurrentUserService currentUserService)
        {
            _managersService = managersService;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            string errorMessage = null;

            if (!request.IsValid(out errorMessage))
                throw new BadRequestException(errorMessage);

            AppUser user = await _managersService.FindByUserNameAsync(_currentUserService.UserName);

            Result result = await _managersService.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded)
                throw new BadRequestException(string.Concat(result.Errors));

            return Unit.Value;
        }
    }
}