using Application.Common.Interfaces;
using Application.Exceptions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdViewModel>
    {
        private readonly IManagersService _managerService;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IManagersService managersService,
                                       IMapper mapper)
        {
            _managerService = managersService;
            _mapper = mapper;
        }

        public async Task<GetUserByIdViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            GetUserByIdViewModel model = await _managerService.FindById(request.Id)
                                                                   .ProjectTo<GetUserByIdViewModel>(_mapper.ConfigurationProvider)
                                                                   .FirstOrDefaultAsync();
            if (model == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, ErrorMessages.DataNotFound);

            return model;
        }
    }
}
