﻿using Application.Common.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Commands.Insert
{
    public class InsertGroupCommandHandler : IRequestHandler<InsertGroupCommand, Group>
    {
        private readonly IApplicationDbContext _context;

        public InsertGroupCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Group> Handle(InsertGroupCommand request, CancellationToken cancellationToken)
        {
            string errorMessage = null;

            if (!request.IsValid(out errorMessage))
                return null;

            Group group = await _context.Groups
                                        .Include(x => x.Connections)
                                        .Where(x => x.Name == request.Name)
                                        .FirstOrDefaultAsync();

            Connection connection = new Connection(request.ConnectionId, request.UserName);

            if (group == null)
            {
                group = new Group(request.Name);

                await _context.Groups.AddAsync(group);
            }
            else
            {
                _context.Groups.Update(group);
            }

            group.Connections.Add(connection);

            return await _context.SaveChangesAsync(cancellationToken) > 0 ? group : null;
        }
    }
}