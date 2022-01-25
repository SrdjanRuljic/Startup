using Application.Common.Interfaces;
using Application.Exceptions;
using Domain.Entities.Identity;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.Delete
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IManagersService _managersService;

        public DeleteUserCommandHandler(IManagersService managerService)
        {
            _managersService = managerService;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            AppUser user = await _managersService.FindByIdAsync(request.Id);

            if (user == null)
                throw new NotFoundException(string.Format(Resources.Translation.EntityWasNotFound, nameof(AppUser), request.Id));

            await _managersService.DeleteUserAsync(user);

            return Unit.Value;
        }
    }
}