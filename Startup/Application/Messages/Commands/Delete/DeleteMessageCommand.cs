using Application.Common.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages.Commands.Delete
{
    [Authorize(Policy = "RequireAuthorization")]
    public class DeleteMessageCommand : IRequest
    {
        public long Id { get; set; }
    }
}