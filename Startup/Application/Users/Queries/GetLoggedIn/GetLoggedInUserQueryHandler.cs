using Application.Common.Interfaces;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities.Identity;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetLoggedIn
{
    public class GetLoggedInUserQueryHandler : IRequestHandler<GetLoggedInUserQuery, GetLoggedInUserViewModel>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IManagersService _managersService;
        private readonly IMapper _mapper;

        public GetLoggedInUserQueryHandler(IManagersService managersService,
                                         ICurrentUserService currentUserService,
                                         IMapper mapper)
        {
            _managersService = managersService;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<GetLoggedInUserViewModel> Handle(GetLoggedInUserQuery request, CancellationToken cancellationToken)
        {
            AppUser user = await _managersService.FindByIdAsync(_currentUserService.UserId);

            if (user == null)
                throw new NotFoundException(string.Format(Resources.Translation.EntityWasNotFound, nameof(AppUser), _currentUserService.UserId));

            GetLoggedInUserViewModel model = _mapper.Map<GetLoggedInUserViewModel>(user);

            return model;
        }
    }
}