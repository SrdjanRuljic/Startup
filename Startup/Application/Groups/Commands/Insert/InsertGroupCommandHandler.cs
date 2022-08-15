using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Commands.Insert
{
    public class InsertGroupCommandHandler : IRequestHandler<InsertGroupCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public InsertGroupCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(InsertGroupCommand request, CancellationToken cancellationToken)
        {
            Group group = new Group(request.Name);

            group.Connections.Add(new Connection(request.ConnectionId, request.UserName));

            await _context.Groups.AddAsync(group);

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}