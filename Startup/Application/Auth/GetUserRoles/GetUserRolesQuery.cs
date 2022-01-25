﻿using Application.Common.Security;
using MediatR;

namespace Application.Auth.GetUserRoles
{
    [Authorize(Policy = "RequireAuthorization")]
    public class GetUserRolesQuery : IRequest<string[]>
    {
    }
}