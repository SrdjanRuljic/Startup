using Application.Common.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Queries.GetGroupByName
{
    public class GetGroupByNameQueryHandler : IRequestHandler<GetGroupByNameQuery, Group>
    {
        private readonly IApplicationDbContext _context;

        public GetGroupByNameQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Group> Handle(GetGroupByNameQuery request, CancellationToken cancellationToken)
        {
            string errorMessage = null;

            if (!request.IsValid(out errorMessage))
                throw new BadRequestException(errorMessage);

            return await _context.Groups
                                 .Include(x => x.Connections)
                                 .Where(x => x.Name == request.Name)
                                 .FirstOrDefaultAsync(cancellationToken);
        }
    }
}