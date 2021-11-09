using Application.Common.Interfaces;
using Domain.Exceptions;
using MediatR;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IManagersService _managersService;

        public UpdateUserCommandHandler(IManagersService managersService)
        {
            _managersService = managersService;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            string errorMessage = null;

            if (!request.IsValid(out errorMessage))
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, errorMessage);

            if (String.IsNullOrEmpty(request.FirstName) || String.IsNullOrWhiteSpace(request.FirstName))
                request.FirstName = null;

            if (String.IsNullOrEmpty(request.LastName) || String.IsNullOrWhiteSpace(request.LastName))
                request.LastName = null;

            return Unit.Value;
        }
    }
}
