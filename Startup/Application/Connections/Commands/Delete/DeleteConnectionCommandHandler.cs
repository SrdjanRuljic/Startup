using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Connections.Commands.Delete
{
    public class DeleteConnectionCommandHandler : IRequestHandler<DeleteConnectionCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteConnectionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteConnectionCommand request, CancellationToken cancellationToken)
        {
            Connection connection = await _context.Connections.FindAsync(request.Id);

            _context.Connections.Remove(connection);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}