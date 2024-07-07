using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Helpers
{
    public class AuthorizationHelper : IAuthorizationHelper
    {
        private const int _saltSize = 16; // 128 bits
        private const int _keySize = 32; // 256 bits
        private const int _iterations = 10000;
        private readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA256;
        private const char segmentDelimiter = ':';

        private const int _tokenValidityTime = 30;
        private readonly IConfiguration _configuration;

        public AuthorizationHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Hashes with structure: [key]:[salt]:[iterations]:[algorithm]
        public string Hash(string input)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(_saltSize);
            byte[] encodedInput = Encoding.UTF8.GetBytes(input);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(encodedInput, salt, _iterations, _algorithm, _keySize);

            return string.Join(segmentDelimiter, Convert.ToHexString(hash), Convert.ToHexString(salt), _iterations, _algorithm);
        }

        public bool ValidateHash(string input, string hashString)
        {
            string[] hashSegments = hashString.Split(segmentDelimiter);
            byte[] hash = Convert.FromHexString(hashSegments[0]);
            byte[] salt = Convert.FromHexString(hashSegments[1]);
            int iterations = int.Parse(hashSegments[2]);
            HashAlgorithmName algorithm = new HashAlgorithmName(hashSegments[3]);

            byte[] hashedInput = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, algorithm, hash.Length);

            return CryptographicOperations.FixedTimeEquals(hashedInput, hash);
        }

        public string GenerateJwtToken()
        {
            string issuerSigningKey = _configuration["Keys:IssuerSigningKey"];
            string validIssuer = _configuration["Keys:ValidIssuer"];

            byte[] bytes = Encoding.ASCII.GetBytes(issuerSigningKey);
            SigningCredentials signingCredentials = new SigningCredentials(new SymmetricSecurityKey(bytes), "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256");

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.Now.AddMinutes(_tokenValidityTime),
                SigningCredentials = signingCredentials,
                Issuer = validIssuer
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(token);
        }
    }
}
