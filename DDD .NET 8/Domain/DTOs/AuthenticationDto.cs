namespace Domain.DTOs
{
    public class AuthenticationDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public AuthenticationDto()
        {
        }

        public AuthenticationDto(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
