using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    public class AppUserRole : IdentityUserRole<string>
    {
        public virtual AppRole Role { get; set; }
        public virtual AppUser User { get; set; }
    }
}