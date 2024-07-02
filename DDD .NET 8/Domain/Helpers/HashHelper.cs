using System;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Helpers
{
    public static class HashHelper
    {
        private const int _saltSize = 16; // 128 bits
        private const int _keySize = 32; // 256 bits
        private const int _iterations = 10000;
        private static readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA256;
        private const char segmentDelimiter = ':';

        // Hashes with structure: [key]:[salt]:[iterations]:[algorithm]
        public static string Hash(string input)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(_saltSize);
            byte[] encodedInput = Encoding.UTF8.GetBytes(input);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(encodedInput, salt, _iterations, _algorithm, _keySize);
            return string.Join(segmentDelimiter, Convert.ToHexString(hash), Convert.ToHexString(salt), _iterations, _algorithm);
        }
    }
}
