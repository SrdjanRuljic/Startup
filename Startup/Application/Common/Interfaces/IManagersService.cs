using Application.Common.Models;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IManagersService
    {
        Task<Result> CreateRoleAsync(string roleName);
        Task<bool> IsThereAnyRoleAsync();
        Task<bool> IsThereAnyUserAsync();
    }
}
