using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities.Identity;
using Infrastructure.Identity.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Infrastructure.Identity
{
    public class ManagersService : IManagersService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserClaimsPrincipalFactory<AppUser> _userClaimsPrincipalFactory;
        private readonly UserManager<AppUser> _userManager;

        public ManagersService(RoleManager<AppRole> roleManager,
                               UserManager<AppUser> userManager,
                               SignInManager<AppUser> signInManager,
                               IUserClaimsPrincipalFactory<AppUser> userClaimsPrincipalFactory,
                               IAuthorizationService authorizationService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _authorizationService = authorizationService;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        }

        public async Task<AppUser> AuthenticateAsync(string userName, string password)
        {
            AppUser user = await _userManager.FindByNameAsync(userName);

            if (user == null) return null;

            SignInResult result = await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);

            return result.Succeeded ? user : null;
        }

        public async Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);

            if (user == null) return false;

            ClaimsPrincipal principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            AuthorizationResult result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
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
            IdentityResult result = await _roleManager.CreateAsync(new AppRole()
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

        public async Task<Result> DeleteUserAsync(AppUser user)
        {
            IdentityResult result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }

        public async Task<AppUser> FindByEmailAsync(string email) =>
            await _userManager.FindByEmailAsync(email);

        public IQueryable<AppUser> FindById(string id) =>
            _userManager.Users.Where(x => x.Id.Equals(id));

        public async Task<AppUser> FindByIdAsync(string id) =>
            await _userManager.FindByIdAsync(id);

        public async Task<AppUser> FindByUserNameAsync(string userName) =>
            await _userManager.FindByNameAsync(userName);

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUser user) =>
            await _userManager.GenerateEmailConfirmationTokenAsync(user);

        public async Task<string> GenerateResetPasswordTokenAsync(AppUser user) =>
            await _userManager.GeneratePasswordResetTokenAsync(user);

        public async Task<string> GetDisplayNameAsync(string userId, CancellationToken cancellationToken) =>
            await _userManager.Users
                              .Where(x => x.Id == userId)
                              .Select(x => SetDisplayName(x.FirstName, x.LastName))
                              .FirstOrDefaultAsync(cancellationToken);

        public async Task<string[]> GetRolesAsync(AppUser user)
        {
            IList<string> list = await _userManager.GetRolesAsync(user);

            return list.ToArray();
        }

        public async Task<AppUser> GetUserByIdAsync(string id, CancellationToken cancellationToken) =>
            await _userManager.Users
                              .Include(x => x.UserRoles)
                              .ThenInclude(x => x.Role)
                              .Where(x => x.Id == id)
                              .FirstOrDefaultAsync(cancellationToken);

        public IQueryable<AppUser> GetUsers() =>
            _userManager.Users;

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);

            return user != null && await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<bool> IsThereAnyRoleAsync(CancellationToken cancellationToken) =>
            await _roleManager.Roles.AnyAsync(cancellationToken);

        public async Task<bool> IsThereAnyUserAsync(CancellationToken cancellationToken) =>
            await _userManager.Users.AnyAsync(cancellationToken);

        public async Task<bool> IsUserInRoleAsync(AppUser user, string role) =>
            await _userManager.IsInRoleAsync(user, role);

        public async Task<Result> ResetPasswordAsync(AppUser user, string token, string password)
        {
            IdentityResult result = await _userManager.ResetPasswordAsync(user, token, password);

            return result.ToApplicationResult();
        }

        public async Task SignOutAsync() =>
            await _signInManager.SignOutAsync();

        public async Task<Result> UpdateUserAsync(AppUser user)
        {
            IdentityResult result = await _userManager.UpdateAsync(user);

            return result.ToApplicationResult();
        }

        public async Task<Result> UpdateUserAsync(AppUser user, string[] roles)
        {
            IdentityResult result = new IdentityResult();

            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    string[] currentRoles = await GetRolesAsync(user);

                    result = await _userManager.RemoveFromRolesAsync(user, currentRoles);

                    if (result.Succeeded)
                    {
                        result = await _userManager.AddToRolesAsync(user, roles);
                    }
                }

                if (result.Succeeded)
                    scope.Complete();
                else
                    scope.Dispose();
            }

            return result.ToApplicationResult();
        }

        public async Task<bool> UserExistAsync(string userName, string email, CancellationToken cancellationToken) =>
            await _userManager.Users.AnyAsync(x => x.UserName.Equals(userName) || x.Email.Equals(email), cancellationToken);

        public async Task<bool> UserNameExistAsync(string id, string userName, CancellationToken cancellationToken) =>
            await _userManager.Users.AnyAsync(x => x.UserName == userName && x.Id != id, cancellationToken);

        private static string SetDisplayName(string firstName, string lastName)
        {
            string displayName = "NN";

            if (string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                displayName = lastName;
            else if (!string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
                displayName = firstName;
            else if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                displayName = firstName + " " + lastName;

            return displayName;
        }
    }
}