using System;
using System.Security.Cryptography;
using System.Text;

namespace CafeManagemnt
{
    public static class PasswordHelper
    {
        private const int SaltSize = 16; // 128 bits
        private const int HashSize = 20; // 160 bits
        private const int Iterations = 10000; // Number of PBKDF2 iterations

        public static string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt;
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt = new byte[SaltSize]);
            }

            // Create the hash using PBKDF2
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Combine salt and hash
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Convert to base64
            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Convert base64 string to bytes
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            // Extract salt
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Compute hash of entered password
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Compare hashes
            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}