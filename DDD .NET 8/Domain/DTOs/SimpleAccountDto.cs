namespace Domain.DTOs
{
    public class SimpleAccountDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Title { get; set; }

        public SimpleAccountDto()
        {
        }

        public SimpleAccountDto(string email, string password, string name, string surname, string title)
        {
            Email = email;
            Password = password;
            Name = name;
            Surname = surname;
            Title = title;
        }
    }
}
