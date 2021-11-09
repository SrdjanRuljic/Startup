﻿using MediatR;

namespace Application.Users.Commands.Update
{
    public class UpdateUserCommand : IRequest
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
