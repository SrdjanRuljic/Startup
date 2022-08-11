using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker _tracker;

        public PresenceHub(PresenceTracker tracker)
        {
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
            await _tracker.UserConnected(GetUserName(), Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOnline", GetUserName());

            string[] currentUsers = await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await _tracker.UserDisconnected(GetUserName(), Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOffline", GetUserName());

            string[] currentUsers = await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);

            await base.OnDisconnectedAsync(exception);
        }

        private string GetUserName()
        {
            ClaimsIdentity clames = Context?.User?.Identity as ClaimsIdentity;

            string userName = clames.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userName;
        }
    }
}