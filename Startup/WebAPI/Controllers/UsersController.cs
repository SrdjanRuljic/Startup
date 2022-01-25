using Application.Common.Pagination.Models;
using Application.Users.Commands.ChangePassword;
using Application.Users.Commands.ChangeUserPassword;
using Application.Users.Commands.Delete;
using Application.Users.Commands.Insert;
using Application.Users.Commands.Update.Self;
using Application.Users.Commands.Update.Update;
using Application.Users.Queries.GetById;
using Application.Users.Queries.GetByUserName;
using Application.Users.Queries.GetDisplayName;
using Application.Users.Queries.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class UsersController : BaseController
    {
        #region [GET]

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            GetUserByIdViewModel user = await Mediator.Send(new GetUserByIdQuery
            {
                Id = id
            });

            return Ok(user);
        }

        [HttpGet]
        [Route("get-by-username")]
        public async Task<IActionResult> GetByUserName(string id)
        {
            GetByUserNameViewModel user = await Mediator.Send(new GetByUserNameQuery());

            return Ok(user);
        }

        [HttpGet]
        [Route("display-name")]
        public async Task<IActionResult> GetDisplayName()
        {
            string displayName = await Mediator.Send(new GetDisplayNameQuery());

            return Ok(new { displayName = displayName });
        }

        #endregion [GET]

        #region [POST]

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Search(SearchUsersQuery query)
        {
            PaginationResultViewModel<SearchUsersViewModel> users = await Mediator.Send(query);

            return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Insert(InsertUserCommand command)
        {
            await Mediator.Send(command);

            return Ok();
        }

        #endregion [POST]

        #region [PUT]

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(UpdateCommand command)
        {
            await Mediator.Send(command);

            return Ok();
        }

        [HttpPut]
        [Route("self")]
        public async Task<IActionResult> UpdateSelf(UpdateSelfCommand command)
        {
            await Mediator.Send(command);

            return Ok();
        }

        [HttpPut]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
        {
            await Mediator.Send(command);

            return Ok();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut]
        [Route("change-user-password")]
        public async Task<IActionResult> ChangeUserPassword(ChangeUserPasswordCommand command)
        {
            await Mediator.Send(command);

            return Ok();
        }

        #endregion [PUT]

        #region [DELETE]

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await Mediator.Send(new DeleteUserCommand
            {
                Id = id
            });

            return Ok();
        }

        #endregion [DELETE]
    }
}