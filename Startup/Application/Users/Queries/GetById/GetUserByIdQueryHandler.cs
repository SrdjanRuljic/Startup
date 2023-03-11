using Application.Common.Interfaces;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities.Identity;
using MediatR;
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
            AppUser user = await _managersService.GetUserByIdAsync(request.Id, cancellationToken);

            if (user == null)
                throw new NotFoundException(string.Format(Resources.Translation.EntityWasNotFound, nameof(AppUser), request.Id));

            GetUserByIdViewModel model = _mapper.Map<GetUserByIdViewModel>(user);

            return model;
        }
    }
}