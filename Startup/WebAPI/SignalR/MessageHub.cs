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
        private readonly ISender _mediator;
        private readonly IHubContext<MessageHub> _messageHubContext;
        private readonly IHubContext<PresenceHub> _presenceHubContext;
        private readonly PresenceTracker _trucker;

        public MessageHub(ISender mediator,
                          IHubContext<MessageHub> messageHubContext,
                          IHubContext<PresenceHub> presenceHubContext,
                          PresenceTracker trucker)
        {
            _mediator = mediator;
            _messageHubContext = messageHubContext;
            _presenceHubContext = presenceHubContext;
            _trucker = trucker;
        }

        public override async Task OnConnectedAsync()
        {
            HttpContext httpContext = Context.GetHttpContext();
            string recipientUserName = httpContext.Request.Query["recipient"].ToString();
            string groupName = GetGroupName(GetUserName(), recipientUserName);

            await _messageHubContext.Groups.AddToGroupAsync(Context.ConnectionId, groupName);

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

            await _messageHubContext.Clients.Groups(groupName).SendAsync("ReciveMessageThread", messages);
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
            {
                message.DateRead = DateTime.UtcNow;
            }
            else
            {
                var connections = await _trucker.GetConnectionsForUser(message.RecipientUserName);
                if (connections != null)
                {
                    await _presenceHubContext.Clients.Clients(connections).SendAsync("NewMessageRecived", message.SenderUserName);
                }
            }

            await _messageHubContext.Clients.Group(groupName).SendAsync("NewMessage", message);
        }

        private async Task<Group> AddToGroup(string groupName)
        {
            bool isAdded = false;
            Group group = null;

            try
            {
                group = await _mediator.Send(new GetGroupByNameQuery()
                {
                    Name = groupName,
                });

                if (group == null)
                {
                    isAdded = await _mediator.Send(new InsertGroupCommand()
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

            return isAdded ? ;
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