using Application.Common.Models;
using Domain.Entities.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IManagersService
    {
        Task<AppUser> AuthenticateAsync(string userName, string password);

        Task<bool> AuthorizeAsync(string userName, string policyName);

        Task<Result> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword);

        Task<Result> ConfirmEmailAsync(AppUser user, string token);

        Task<Result> CreateRoleAsync(string roleName);

        Task<Result> CreateUserAsync(AppUser user, string password, string[] roles);

        Task<Result> DeleteUserAsync(AppUser user);

        Task<AppUser> FindByEmailAsync(string email);

        IQueryable<AppUser> FindById(string id);

        Task<AppUser> FindByIdAsync(string id);

        Task<AppUser> FindByUserNameAsync(string userName);

        Task<string> GenerateEmailConfirmationTokenAsync(AppUser user);

        Task<string> GenerateResetPasswordTokenAsync(AppUser user);

        Task<string> GetDisplayNameAsync(string userName);

        Task<string[]> GetRolesAsync(AppUser user);

        Task<AppUser> GetUserByIdAsync(string id);

        IQueryable<AppUser> GetUsers();

        Task<bool> IsInRoleAsync(string userName, string role);

        Task<bool> IsThereAnyRoleAsync();

        Task<bool> IsThereAnyUserAsync();

        Task<bool> IsUserInRoleAsync(AppUser user, string role);

        Task<Result> ResetPasswordAsync(AppUser user, string token, string password);

        Task SignOutAsync();

        Task<Result> UpdateUserAsync(AppUser user);

        Task<Result> UpdateUserAsync(AppUser user, string[] roles);

        Task<bool> UserExistAsync(string userName, string email);

        Task<bool> UserNameExistAsync(string id, string userName);
    }
}