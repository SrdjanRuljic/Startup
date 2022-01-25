﻿using Application.Common.Interfaces;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities.Identity;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetByUserName
{
    public class GetByUserNameQueryHandler : IRequestHandler<GetByUserNameQuery, GetByUserNameViewModel>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IManagersService _managersService;
        private readonly IMapper _mapper;

        public GetByUserNameQueryHandler(IManagersService managersService,
                                         ICurrentUserService currentUserService,
                                         IMapper mapper)
        {
            _managersService = managersService;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<GetByUserNameViewModel> Handle(GetByUserNameQuery request, CancellationToken cancellationToken)
        {
            AppUser user = await _managersService.FindByUserNameAsync(_currentUserService.UserName);

            if (user == null)
                throw new NotFoundException(string.Format(Resources.Translation.EntityWasNotFound, nameof(AppUser), _currentUserService.UserName));

            GetByUserNameViewModel model = _mapper.Map<GetByUserNameViewModel>(user);

            return model;
        }
    }
}