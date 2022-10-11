using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Domain.Entities.Identity
{
    public class AppRole : IdentityRole
    {
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
    }
}