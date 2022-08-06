using Application.Common.Pagination.Models;
using Application.Messages.Commands.Insert;
using Application.Messages.Commands.ReadMany;
using Application.Messages.Queries.GetThread;
using Application.Messages.Queries.Search;
using Application.Messages.Queries.SearchInterlocutors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class MessagesController : BaseController
    {
        #region [GET]

        [HttpGet]
        [Route("thread/{username}")]
        public async Task<IActionResult> Thread(string username)
        {
            bool markAsReaded = await Mediator.Send(new ReadManyMassagesCommand()
            {
                RecipientUserName = username
            });

            IEnumerable<GetMessageThreadQueryViewModel> messages = await Mediator.Send(new GetMessageThreadQuery()
            {
                RecipientUserName = username
            });

            return Ok(messages);
        }

        #endregion [GET]

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

        [HttpPost]
        [Route("search-interlocutors")]
        public async Task<IActionResult> SearchInterlocutors(SearchInterlocutorsQuery query)
        {
            PaginationResultViewModel<SearchInterlocutorsViewModel> interlocutors = await Mediator.Send(query);

            return Ok(interlocutors);
        }

        #endregion [POST]
    }
}