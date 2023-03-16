using Application.Common.Pagination.Models;
using Application.Messages.Commands.Delete;
using Application.Messages.Commands.Insert;
using Application.Messages.Queries.Search;
using Application.Messages.Queries.SearchInterlocutors;
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
            await Sender.Send(command);

            return Ok();
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Search(SearchMessagesQuery query)
        {
            PaginationResultViewModel<SearchMessagesQueryViewModel> messages = await Sender.Send(query);

            return Ok(messages);
        }

        [HttpPost]
        [Route("search-interlocutors")]
        public async Task<IActionResult> SearchInterlocutors(SearchInterlocutorsQuery query)
        {
            PaginationResultViewModel<SearchInterlocutorsViewModel> interlocutors = await Sender.Send(query);

            return Ok(interlocutors);
        }

        #endregion [POST]

        #region [DELETE]

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await Sender.Send(new DeleteMessageCommand
            {
                Id = id
            });

            return Ok();
        }

        #endregion [DELETE]
    }
}