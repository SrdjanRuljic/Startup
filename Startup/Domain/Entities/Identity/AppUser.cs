﻿using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
