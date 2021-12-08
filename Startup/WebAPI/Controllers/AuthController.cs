﻿using Application.Auth.ConfirmEmail;
using Application.Auth.IsInRole;
using Application.Auth.Queries.Login;
using Application.Auth.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class AuthController : BaseController
    {
        #region [GET]

        [HttpGet]
        [Route("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string username, [FromQuery] string token)
        {
            await Mediator.Send(new ConfirmEmailCommand()
            {
                Token = token,
                UserName = username
            });

            return Ok();
        }

        [HttpGet]
        [Route("is-in-role/{role}")]
        [AllowAnonymous]
        public async Task<IActionResult> IsInRole(string role)
        {
            bool isInRole = await Mediator.Send(new IsInRoleQuery()
            {
                Role = role
            });

            return Ok(isInRole);
        }

        #endregion

        #region [POST]

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginQuery query)
        {
            object token = await Mediator.Send(query);

            return Ok(token);
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            await Mediator.Send(command);

            return Ok();
        }

        #endregion        
    }
}
