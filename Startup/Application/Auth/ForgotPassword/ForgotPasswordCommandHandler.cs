using Application.Common.Behaviours;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Exceptions;
using Domain.Entities.Identity;
using Domain.Exceptions;
using MediatR;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IManagersService _managersService;
        private readonly IEmailSenderService _emailSenderService;

        public ForgotPasswordCommandHandler(IManagersService managersService,
                                            IEmailSenderService emailSenderService)
        {
            _managersService = managersService;
            _emailSenderService = emailSenderService;
        }

        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            string errorMessage = null;

            if (!request.IsValid(out errorMessage))
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, errorMessage);

            AppUser user = await _managersService.FindByEmailAsync(request.Email);

            if (user == null)
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, ErrorMessages.DataNotFound);

            string token = await _managersService.GenerateResetPasswordTokenAsync(user);
            string link = LinkMaker.CreateResetPasswordLink(user.Email, request.ClientUri, token);

            Message message = new Message(new string[] { user.Email }, "Reset password request", link, null);

            await _emailSenderService.SendResetPasswordEmailAsync(message);

            return Unit.Value;
        }
    }
}