namespace Application.Common.Interfaces
{
    public interface IJwtFactory
    {
        string GenerateEncodedToken();

        string GenerateEncodedToken(string username, string[] roles);

        bool Validate(string refreshToken);
    }
}