namespace Partpurja.Application.Interface.IServices
{
    public interface ITokenService
    {
        string GenerateToken(string username, string role);
    }
}
