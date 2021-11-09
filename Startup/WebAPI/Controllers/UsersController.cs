using Application.Common.Pagination.Models;
using Application.Users.Commands.Insert;
using Application.Users.Queries.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class UsersController : BaseController
    {
        #region [POST]

        [Authorize(Policy = "RequireAdminRole")]
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

        #endregion
    }
}
