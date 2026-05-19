namespace Partpurja.Application.Interface.IServices
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string stored);
    }
}
