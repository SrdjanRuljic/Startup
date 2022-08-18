using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Connections.Commands.Delete
{
    public class DeleteConnectionCommandHandler : IRequestHandler<DeleteConnectionCommand, Group>
    {
        private readonly IApplicationDbContext _context;

        public DeleteConnectionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Group> Handle(DeleteConnectionCommand request, CancellationToken cancellationToken)
        {
            Group group = await _context.Groups
                                        .Include(x => x.Connections)
                                        .Where(x => x.Connections.Any(x => x.Id == request.Id))
                                        .FirstOrDefaultAsync();

            Connection connection = group.Connections
                                         .Where(x => x.Id == request.Id)
                                         .FirstOrDefault();

            _context.Connections.Remove(connection);

            return await _context.SaveChangesAsync(cancellationToken) > 0 ? group : null;
        }
    }
}