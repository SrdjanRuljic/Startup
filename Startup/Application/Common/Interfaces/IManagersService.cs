using Application.Common.Models;
using Domain.Entities.Identity;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IManagersService
    {
        Task<AppUser> AuthenticateAsync(string userName, string password);
        Task<Result> ConfirmEmailAsync(AppUser user, string token);
        Task<Result> CreateRoleAsync(string roleName);
        Task<Result> CreateUserAsync(AppUser user, string password, string role);
        Task<AppUser> FindByUserNameAsync(string userName);
        Task<string[]> GetRoleAsync(AppUser user);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser user);
        Task<bool> IsThereAnyRoleAsync();
        Task<bool> IsThereAnyUserAsync();
        Task<bool> UserExist(string userName, string email);
    }
}
