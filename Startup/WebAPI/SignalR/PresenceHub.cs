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
        public override async Task OnConnectedAsync()
        {

            await Clients.Others.SendAsync("UserIsOnline", GetUserName());
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.Others.SendAsync("UserIsOffline", GetUserName());

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