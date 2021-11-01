using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IManagersService
    {
        Task<bool> IsThereAnyRoleAsync();
        Task CreateRoleAsync(string roleName);
    }
}
