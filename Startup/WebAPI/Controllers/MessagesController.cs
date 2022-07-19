using Application.Common.Pagination.Models;
using Application.Messages.Commands.Insert;
using Application.Messages.Queries.Search;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class MessagesController : BaseController
    {
        #region [POST]

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Insert(InsertMessageCommand command)
        {
            await Mediator.Send(command);

            return Ok();
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Search(SearchMessagesQuery query)
        {
            PaginationResultViewModel<SearchMessagesQueryViewModel> messages = await Mediator.Send(query);

            return Ok(messages);
        }

        #endregion [POST]
    }
}