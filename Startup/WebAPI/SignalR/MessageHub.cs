using Application.Common.Interfaces;
using Application.Messages;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.SignalR
{
    public class MessageHub : Hub, IMessageHub
    {
        private readonly IHubContext<MessageHub> _context;

        public MessageHub(IHubContext<MessageHub> context)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            HttpContext httpContext = Context.GetHttpContext();
            string recipientUserName = httpContext.Request.Query["recipient"].ToString();
            string groupName = GetGroupName(GetUserName(), recipientUserName);

            await _context.Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(MessageViewModel message)
        {
            var groupName = GetGroupName(message.SenderUserName, message.RecipientUserName);

            await _context.Clients.Group(groupName).SendAsync("NewMessage", message);
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
    }
}