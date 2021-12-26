using Application.Common.Models;
using Domain.Entities.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IManagersService
    {
        Task<AppUser> AuthenticateAsync(string userName, string password);
        Task<Result> ConfirmEmailAsync(AppUser user, string token);
        Task<Result> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword);
        Task<Result> CreateRoleAsync(string roleName);
        Task<Result> CreateUserAsync(AppUser user, string password, string[] roles);
        Task<Result> DeleteUserAsync(AppUser user);
        Task<AppUser> FindByUserNameAsync(string userName);
        Task<string> GetDisplayNameAsync(string userName);
        Task<AppUser> FindByIdAsync(string id);
        IQueryable<AppUser> FindById(string id);
        Task<string[]> GetRoleAsync(AppUser user);
        IQueryable<AppUser> GetUsers();
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser user);
        Task<bool> IsThereAnyRoleAsync();
        Task<bool> IsThereAnyUserAsync();
        Task<bool> IsUserInRoleAsync(AppUser user, string role);
        Task<Result> UpdateUserAsync(AppUser user);
        Task<Result> UpdateUserAsync(AppUser user, string[] roles);
        Task<bool> UserExistAsync(string userName, string email);
        Task<bool> UserNameExistAsync(string id, string userName);
    }
}
