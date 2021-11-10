using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Exceptions;
using Domain.Entities.Identity;
using Domain.Exceptions;
using MediatR;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand>
    {
        private readonly IManagersService _managersService;

        public ConfirmEmailCommandHandler(IManagersService managersService)
        {
            _managersService = managersService;
        }

        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            AppUser user = await _managersService.FindByUserNameAsync(request.UserName);

            if (user == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, ErrorMessages.DataNotFound);

            Result result = await _managersService.ConfirmEmailAsync(user, request.Token);

            if (!result.Succeeded)
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, string.Concat(result.Errors));

            return Unit.Value;
        }
    }
}
