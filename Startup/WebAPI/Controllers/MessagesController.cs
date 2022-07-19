using Application.Messages.Commands.Insert;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class MessagesController : BaseController
    {
        #region [POST]

        //[HttpPost]
        //[Route("search")]
        //public async Task<IActionResult> Search(SearchUsersQuery query)
        //{
        //    PaginationResultViewModel<SearchUsersViewModel> users = await Mediator.Send(query);

        //    return Ok(users);
        //}

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Insert(InsertMessageCommand command)
        {
            await Mediator.Send(command);

            return Ok();
        }

        #endregion [POST]
    }
}