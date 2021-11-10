using Application.Common.Interfaces;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities.Identity;
using Domain.Exceptions;
using MediatR;
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
            AppUser user = await _managerService.FindByIdAsync(request.Id);

            if (user == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, ErrorMessages.DataNotFound);

            GetUserByIdViewModel model = _mapper.Map<GetUserByIdViewModel>(user);

            model.Roles = await _managerService.GetRoleAsync(user);

            return model;
        }
    }
}
