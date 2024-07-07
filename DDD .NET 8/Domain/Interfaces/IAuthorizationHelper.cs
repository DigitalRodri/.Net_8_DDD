namespace Domain.Interfaces
{
    public interface IAuthorizationHelper
    {
        string GenerateJwtToken();
        string Hash(string input);
        bool ValidateHash(string input, string hashString);
    }
}