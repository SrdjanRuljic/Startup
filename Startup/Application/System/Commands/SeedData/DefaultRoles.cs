using Application.Common.Interfaces;
using System.Threading.Tasks;

namespace Application.System.Commands.SeedData
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(IManagersService _managersService)
        {
            //Seed Roles
            await _managersService.CreateRoleAsync(Domain.Enums.Roles.Admin.ToString());
            await _managersService.CreateRoleAsync(Domain.Enums.Roles.Moderator.ToString());
            await _managersService.CreateRoleAsync(Domain.Enums.Roles.Basic.ToString());
        }
    }
}