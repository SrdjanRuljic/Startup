using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace WebAPI.SignalR
{
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker _tracker;

        public PresenceHub(PresenceTracker tracker)
        {
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
            bool isOnline = await _tracker.UserConnected(GetUserId(), Context.ConnectionId);

            if (isOnline)
                await Clients.Others.SendAsync("UserIsOnline", GetUserId());

            string[] currentUsers = await _tracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            bool isOffline = await _tracker.UserDisconnected(GetUserId(), Context.ConnectionId);

            if (isOffline)
                await Clients.Others.SendAsync("UserIsOffline", GetUserId());

            await base.OnDisconnectedAsync(exception);
        }

        private string GetUserId()
        {
            ClaimsIdentity clames = Context?.User?.Identity as ClaimsIdentity;

            string userId = clames.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId;
        }
    }
}