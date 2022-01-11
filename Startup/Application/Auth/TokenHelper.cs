using Application.Common.Interfaces;

namespace Application.Auth
{
    public static class TokenHelper
    {
        public static object GenerateJwt(string username, string[] roles, IJwtFactory jwtFactory) => new
        {
            auth_token = jwtFactory.GenerateEncodedToken(username, roles)
        };
    }
}