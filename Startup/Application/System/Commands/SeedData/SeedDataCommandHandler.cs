using Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.System.Commands.SeedData
{
    public class SeedDataCommandHandler : IRequestHandler<SeedDataCommand>
    {
        private readonly IManagersService _managersService;

        public SeedDataCommandHandler(IManagersService managersService)
        {
            _managersService = managersService;
        }

        public async Task<Unit> Handle(SeedDataCommand request, CancellationToken cancellationToken)
        {
            DataSeeder seeder = new DataSeeder(_managersService);

            await seeder.SeedAllAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
