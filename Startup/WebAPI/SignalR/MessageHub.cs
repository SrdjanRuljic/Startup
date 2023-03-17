using Application.Connections.Commands.Delete;
using Application.Groups.Commands.Insert;
using Application.Groups.Queries.GetGroupByName;
using Application.Messages.Commands.Insert;
using Application.Messages.Commands.ReadMany;
using Application.Messages.Queries.GetThread;
using Application.Messages;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using Domain.Entities;
using System.Linq;
using Application.Common.Interfaces;

namespace WebAPI.SignalR
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly ISender _mediator;
        private readonly IHubContext<PresenceHub> _presenceHubContext;
        private readonly PresenceTracker _trucker;

        public MessageHub(IDateTimeService dateTimeService,
                          ISender mediator,
                          IHubContext<PresenceHub> presenceHubContext,
                          PresenceTracker trucker)
        {
            _dateTimeService = dateTimeService;
            _mediator = mediator;
            _presenceHubContext = presenceHubContext;
            _trucker = trucker;
        }

        public override async Task OnConnectedAsync()
        {
            HttpContext httpContext = Context.GetHttpContext();
            string recipientUserId = httpContext.Request.Query["recipient"].ToString();
            string groupName = GetGroupName(GetUserId(), recipientUserId);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            Group group = await _mediator.Send(new InsertGroupCommand()
            {
                ConnectionId = Context.ConnectionId,
                Name = groupName,
                UserId = GetUserId()
            });

            if (group == null)
                throw new HubException("Faild to join group!");

            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            try
            {
                bool markAsReaded = await _mediator.Send(new ReadManyMassagesCommand()
                {
                    RecipientUserId = recipientUserId
                });
            }
            catch (Exception exception)
            {
                throw new HubException(exception.Message);
            }

            IEnumerable<MessageViewModel> messages = await _mediator.Send(new GetMessageThreadQuery()
            {
                RecipientUserId = recipientUserId
            });

            await Clients.Caller.SendAsync("ReciveMessageThread", messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Group group = await _mediator.Send(new DeleteConnectionCommand()
            {
                Id = Context.ConnectionId
            });

            if (group == null)
                throw new HubException("Faild to remove from group!");

            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string recipientUserId, string content)
        {
            MessageViewModel message = null;
            string groupName = null;
            Group group = null;

            try
            {
                message = await _mediator.Send(new InsertMessageCommand()
                {
                    Content = content,
                    RecipientUserId = recipientUserId
                });

                groupName = GetGroupName(message.SenderId, message.RecipientId);

                group = await _mediator.Send(new GetGroupByNameQuery()
                {
                    Name = groupName,
                });
            }
            catch (Exception exception)
            {
                throw new HubException(exception.Message);
            }

            if (group?.Connections.Any(x => x.UserId == message.RecipientId) == true)
            {
                message.DateRead = _dateTimeService.Now;
            }
            else
            {
                var connections = await _trucker.GetConnectionsForUser(message.RecipientId);
                if (connections != null)
                {
                    await _presenceHubContext.Clients.Clients(connections).SendAsync("NewMessageRecived", message.SenderId);
                }
            }

            await Clients.Group(groupName).SendAsync("NewMessage", message);
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;

            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

        private string GetUserId()
        {
            ClaimsIdentity clames = Context?.User?.Identity as ClaimsIdentity;

            string userId = clames.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId;
        }
    }
}