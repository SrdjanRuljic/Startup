﻿using Application.Common.Interfaces;
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

        public async Task<AppUser> AuthenticateAsync(string userName, string password)
        {
            AppUser user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return null;

            SignInResult result = await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);

            return result.Succeeded ? user : null;
        }

        public async Task<Result> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword)
        {
            IdentityResult result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            return result.ToApplicationResult();
        }

        public async Task<Result> ConfirmEmailAsync(AppUser user, string token)
        {
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);

            return result.ToApplicationResult();
        }

        public async Task<Result> CreateRoleAsync(string roleName)
        {
            IdentityResult result = await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = roleName
            });

            return result.ToApplicationResult();
        }

        public async Task<Result> CreateUserAsync(AppUser user, string password, string[] roles)
        {
            IdentityResult result = new IdentityResult();

            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    foreach (var role in roles)
                    {
                        result = await _userManager.AddToRoleAsync(user, role);
                    }
                }

                if (result.Succeeded)
                    scope.Complete();
                else
                    scope.Dispose();
            }

            return result.ToApplicationResult();
        }

        public async Task<AppUser> FindByUserNameAsync(string userName) =>
            await _userManager.FindByNameAsync(userName);

        public async Task<AppUser> FindByIdAsync(string id) =>
            await _userManager.FindByIdAsync(id);

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUser user) =>
            await _userManager.GenerateEmailConfirmationTokenAsync(user);

        public async Task<string[]> GetRoleAsync(AppUser user)
        {
            IList<string> list = await _userManager.GetRolesAsync(user);

            return list.ToArray();
        }

        public IQueryable<AppUser> GetUsers() =>
            _userManager.Users;

        public async Task<bool> IsThereAnyRoleAsync() =>
            await _roleManager.Roles.AnyAsync();

        public async Task<bool> IsThereAnyUserAsync() =>
            await _userManager.Users.AnyAsync();

        public async Task<Result> UpdateUserAsync(AppUser user)
        {
            IdentityResult result = await _userManager.UpdateAsync(user);

            return result.ToApplicationResult();
        }

        public async Task<bool> UserExistAsync(string userName, string email) =>
            await _userManager.Users.AnyAsync(x => x.UserName.Equals(userName) || x.Email.Equals(email));

        public async Task<bool> UserNameExistAsync(string id, string userName) =>
            await _userManager.Users.AnyAsync(x => x.UserName.Equals(userName) && !x.Id.Equals(id));

        private async Task<IdentityResult> DeleteUserAsync(AppUser user) =>
            await _userManager.DeleteAsync(user);
    }
}
