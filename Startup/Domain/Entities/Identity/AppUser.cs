using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Domain.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Message> MessagesRecived { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
    }
}