using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace WebAPI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string UserId
        {
            get
            {
                ClaimsIdentity clames = _httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;

                string userId = clames?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return userId;
            }
        }

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}