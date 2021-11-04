using Application.Common.Models;
using Domain.Entities.Identity;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IManagersService
    {
        Task<AppUser> AuthenticateAsync(string username, string password);
        Task<Result> CreateRoleAsync(string roleName);
        Task<Result> CreateUserAsync(AppUser user, string password, string role);
        Task<AppUser> FindByUserNameAsync(string username);
        Task<string[]> GetRoleAsync(AppUser user);
        Task<bool> IsThereAnyRoleAsync();
        Task<bool> IsThereAnyUserAsync();
        Task<bool> UserExist(string userName, string email);
    }
}
