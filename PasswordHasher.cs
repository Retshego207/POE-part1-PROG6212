using System.Security.Cryptography;

namespace RaceDayAPI
{
    public class PasswordHasher
    {
        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                600000,
                HashAlgorithmName.SHA256,
                32);
            return $"PBKDF2${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }
        public bool VerifyPassword(string password, string storedPassword)
        {
            string[] parts = storedPassword.Split('$');

            if (parts.Length != 3 || parts[0] != "PBKDF2")
            {
                return false ;
            }
            try
            {
                byte[] salt = Convert.FromBase64String(parts[1]);
                byte[] expectedHash = Convert.FromBase64String(parts[2]);

                byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
                    password,
                    salt,
                    600000,
                    HashAlgorithmName.SHA256,
                    32);

                return CryptographicOperations.FixedTimeEquals(
                    actualHash,
                    expectedHash);
            }
            catch (FormatException)
            {
                return false;
            }
            }
    }
}
