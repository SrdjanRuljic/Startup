using Application.Common.Interfaces;

namespace Application.Auth
{
    public static class TokenHelper
    {
        public static object GenerateJwt(string userId, string[] roles, IJwtFactory jwtFactory) => new
        {
            auth_token = jwtFactory.GenerateEncodedToken(userId, roles),
            refresh_token = jwtFactory.GenerateEncodedToken()
        };
    }
}