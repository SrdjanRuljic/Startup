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
        private readonly IManagersService _managersService;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IManagersService managersService,
                                       IMapper mapper)
        {
            _managersService = managersService;
            _mapper = mapper;
        }

        public async Task<GetUserByIdViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            AppUser user = await _managersService.FindByIdAsync(request.Id);

            if (user == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, ErrorMessages.DataNotFound);

            GetUserByIdViewModel model = _mapper.Map<GetUserByIdViewModel>(user);

            model.Roles = await _managersService.GetRoleAsync(user);

            return model;
        }
    }
}