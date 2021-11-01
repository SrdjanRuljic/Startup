using Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Application.System.Commands.SeedData
{
    public class DataSeeder
    {
        private readonly IManagersService _managersService;

        public DataSeeder(IManagersService managersService)
        {
            _managersService = managersService;
        }

        public async Task SeedAllAsync(CancellationToken cancellationToken)
        {
            if (!await _managersService.IsThereAnyRoleAsync())
                await SeedRolesAsync();
            else
                return;
        }

        public async Task SeedRolesAsync()
        {
            await DefaultRoles.SeedAsync(_managersService);
        }
    }
}
