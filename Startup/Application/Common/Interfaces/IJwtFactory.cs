namespace Application.Common.Interfaces
{
    public interface IJwtFactory
    {
        string GenerateEncodedToken(string username, string[] roles);
    }
}
