namespace Domain.DTOs
{
    public class AccountDto
    {
        public Guid UUID { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Title { get; set; }

        public AccountDto()
        {
        }

        public AccountDto(Guid UUID, string email, string name, string surname, string title)
        {
            this.UUID = UUID;
            Email = email;
            Name = name;
            Surname = surname;
            Title = title;
        }
    }
}
