using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities.Identity;
using Infrastructure.Identity.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class ManagersService : IManagersService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public ManagersService(RoleManager<IdentityRole> roleManager,
                               UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<Result> CreateRoleAsync(string roleName)
        {
            IdentityResult result = await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = roleName
            });

            return result.ToApplicationResult();
        }

        public async Task<Result> CreateUserAsync(AppUser user, string password, string role)
        {
            IdentityResult result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return result.ToApplicationResult();

            result = await _userManager.AddToRoleAsync(user, role);

            return result.ToApplicationResult();
        }

        public async Task<AppUser> FindByUserNameAsync(string username) =>
            await _userManager.FindByNameAsync(username);

        public async Task<bool> IsThereAnyRoleAsync() =>
            await _roleManager.Roles.AnyAsync();

        public async Task<bool> IsThereAnyUserAsync() =>
            await _userManager.Users.AnyAsync();

        private async Task<IdentityResult> DeleteUserAsync(AppUser user) =>
            await _userManager.DeleteAsync(user);
    }
}
