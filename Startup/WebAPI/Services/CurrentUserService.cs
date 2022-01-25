using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace WebAPI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        //public string UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

        public string UserName
        {
            get
            {
                ClaimsIdentity clames = _httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;

                string userName = clames.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return userName;
            }
        }

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}