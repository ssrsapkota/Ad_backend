using System.Security.Cryptography;
using Partpurja.Application.Interface.IServices;

namespace Partpurja.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100_000;

        public string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = DeriveKey(password, salt);
            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        public bool Verify(string password, string stored)
        {
            var parts = stored.Split(':');
            if (parts.Length != 2) return false;

            try
            {
                var salt = Convert.FromBase64String(parts[0]);
                var expected = Convert.FromBase64String(parts[1]);
                var actual = DeriveKey(password, salt);
                return CryptographicOperations.FixedTimeEquals(actual, expected);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static byte[] DeriveKey(string password, byte[] salt)
        {
            return Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
        }
    }
}
