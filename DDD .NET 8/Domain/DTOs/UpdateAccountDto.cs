namespace Domain.DTOs
{
    public class UpdateAccountDto
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Title { get; set; }

        public UpdateAccountDto()
        {
        }

        public UpdateAccountDto(string email, string name, string surname, string title)
        {
            Email = email;
            Name = name;
            Surname = surname;
            Title = title;
        }
    }
}
