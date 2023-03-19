using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Queries.GetForConnection
{
    public class GetForConnectionQueryHandler : IRequestHandler<GetForConnectionQuery, Group>
    {
        private readonly IApplicationDbContext _context;

        public GetForConnectionQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Group> Handle(GetForConnectionQuery request, CancellationToken cancellationToken)
        {
            return await _context.Groups
                                 .Include(x => x.Connections)
                                 .Where(x => x.Connections.Any(x => x.Id == request.ConnectionId))
                                 .FirstOrDefaultAsync(cancellationToken);
        }
    }
}