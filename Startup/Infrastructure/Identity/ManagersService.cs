using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities.Identity;
using Infrastructure.Identity.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Infrastructure.Identity
{
    public class ManagersService : IManagersService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public ManagersService(RoleManager<IdentityRole> roleManager,
                               UserManager<AppUser> userManager,
                               SignInManager<AppUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<AppUser> AuthenticateAsync(string username, string password)
        {
            AppUser user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return null;

            SignInResult result = await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);

            return result.Succeeded ? user : null;
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
            IdentityResult result = new IdentityResult();

            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                    result = await _userManager.AddToRoleAsync(user, role);

                if (result.Succeeded)
                    scope.Complete();
                else
                    scope.Dispose();
            }

            return result.ToApplicationResult();
        }

        public async Task<AppUser> FindByUserNameAsync(string username) =>
            await _userManager.FindByNameAsync(username);

        public async Task<string[]> GetRoleAsync(AppUser user)
        {
            IList<string> list = await _userManager.GetRolesAsync(user);

            return list.ToArray();
        }

        public async Task<bool> IsThereAnyRoleAsync() =>
            await _roleManager.Roles.AnyAsync();

        public async Task<bool> IsThereAnyUserAsync() =>
            await _userManager.Users.AnyAsync();

        private async Task<IdentityResult> DeleteUserAsync(AppUser user) =>
            await _userManager.DeleteAsync(user);
    }
}
