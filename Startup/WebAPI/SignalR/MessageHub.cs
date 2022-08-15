using Application.Connections.Commands.Delete;
using Application.Groups.Commands.Insert;
using Application.Groups.Queries.GetGroupByName;
using Application.Messages;
using Application.Messages.Commands.Insert;
using Application.Messages.Commands.ReadMany;
using Application.Messages.Queries.GetThread;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.SignalR
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IHubContext<MessageHub> _context;
        private readonly ISender _mediator;

        public MessageHub(IHubContext<MessageHub> context, ISender mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public override async Task OnConnectedAsync()
        {
            HttpContext httpContext = Context.GetHttpContext();
            string recipientUserName = httpContext.Request.Query["recipient"].ToString();
            string groupName = GetGroupName(GetUserName(), recipientUserName);

            await _context.Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await AddToGroup(groupName);

            try
            {
                bool markAsReaded = await _mediator.Send(new ReadManyMassagesCommand()
                {
                    RecipientUserName = recipientUserName
                });
            }
            catch (Exception exception)
            {
                throw new HubException(exception.Message);
            }

            IEnumerable<MessageViewModel> messages = await _mediator.Send(new GetMessageThreadQuery()
            {
                RecipientUserName = recipientUserName
            });

            await _context.Clients.Groups(groupName).SendAsync("ReciveMessageThread", messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await RemoveFromGroup();

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string recipientUserName, string content)
        {
            MessageViewModel message = null;
            string groupName = null;
            Group group = null;

            try
            {
                message = await _mediator.Send(new InsertMessageCommand()
                {
                    Content = content,
                    RecipientUserName = recipientUserName
                });

                groupName = GetGroupName(message.SenderUserName, message.RecipientUserName);

                group = await _mediator.Send(new GetGroupByNameQuery()
                {
                    Name = groupName,
                });
            }
            catch (Exception exception)
            {
                throw new HubException(exception.Message);
            }

            if (group?.Connections.Any(x => x.UserName == message.RecipientUserName) == true)
                message.DateRead = DateTime.UtcNow;

            await _context.Clients.Group(groupName).SendAsync("NewMessage", message);
        }

        private async Task<bool> AddToGroup(string groupName)
        {
            bool idAdded = false;

            try
            {
                Group group = await _mediator.Send(new GetGroupByNameQuery()
                {
                    Name = groupName,
                });

                if (group == null)
                {
                    idAdded = await _mediator.Send(new InsertGroupCommand()
                    {
                        ConnectionId = Context.ConnectionId,
                        Name = groupName,
                        UserName = GetUserName()
                    });
                }
            }
            catch (Exception exception)
            {
                throw new HubException(exception.Message);
            }

            return idAdded;
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;

            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

        private string GetUserName()
        {
            ClaimsIdentity clames = Context?.User?.Identity as ClaimsIdentity;

            string userName = clames.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userName;
        }

        private async Task RemoveFromGroup()
        {
            await _mediator.Send(new DeleteConnectionCommand()
            {
                Id = Context.ConnectionId
            });
        }
    }
}