namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Account", Schema="account")]
    public partial class Account
    {
        [Key]
        [Required]
        [Column(TypeName = "uniqueidentifier")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UUID { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(128)]
        public string Password { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(25)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(25)]
        public string Surname { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(5)]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime UTCCreatedDateTime { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UTCUpdatedDateTime { get; set; }

        public Account()
        {
        }

        public Account(Guid UUID, string email, string password, string name, string surname, string title, DateTime uTCCreatedDateTime, DateTime uTCUpdatedDateTime)
        {
            this.UUID = UUID;
            Email = email;
            Password = password;
            Name = name;
            Surname = surname;
            Title = title;
            UTCCreatedDateTime = uTCCreatedDateTime;
            UTCUpdatedDateTime = uTCUpdatedDateTime;
        }

        public Account(string email, string password, string name, string surname, string title)
        {
            Email = email;
            Password = password;
            Name = name;
            Surname = surname;
            Title = title;
        }
    }
}
