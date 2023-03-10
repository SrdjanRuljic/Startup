using Application.Auth.Commands.ConfirmEmail;
using Application.Auth.Commands.ForgotPassword;
using Application.Auth.Queries.GetUserRoles;
using Application.Auth.Commands.Register;
using Application.Auth.Commands.ResetPassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Auth.Commands.Login;
using Application.Auth.Commands.Logout;
using Application.Auth.Commands.Refresh;

namespace WebAPI.Controllers
{
    public class AuthController : BaseController
    {
        #region [GET]

        [HttpGet]
        [Route("user-roles")]
        public async Task<IActionResult> UserRoles()
        {
            string[] roles = await Mediator.Send(new GetUserRolesQuery());

            return Ok(roles);
        }

        #endregion [GET]

        #region [POST]

        [HttpPost]
        [Route("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand command)
        {
            await Mediator.Send(command);

            return Ok();
        }

        [HttpPost]
        [Route("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command)
        {
            await Mediator.Send(command);

            return Ok();
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            object token = await Mediator.Send(command);

            return Ok(token);
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await Mediator.Send(new LogoutCommand());

            return Ok();
        }

        [HttpPost]
        [Route("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
        {
            object result = await Mediator.Send(command);

            return Ok(result);
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            await Mediator.Send(command);

            return Ok();
        }

        [HttpPost]
        [Route("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            await Mediator.Send(command);

            return Ok();
        }

        #endregion [POST]
    }
}