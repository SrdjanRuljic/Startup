using Domain.Entities.Identity;

namespace Domain.Entities
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public AppUser User { get; set; }
        public string UserId { get; set; }

        public RefreshToken(string userId, string token)
        {
            UserId = userId;
            Token = token;
        }

        public RefreshToken()
        {
        }
    }
}