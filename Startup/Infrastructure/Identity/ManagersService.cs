using Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class ManagersService : IManagersService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManagersService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task CreateRoleAsync(string roleName) =>
            await _roleManager.CreateAsync(new IdentityRole() { Name = roleName });

        public async Task<bool> IsThereAnyRoleAsync() =>
            await _roleManager.Roles.AnyAsync();
    }
}
