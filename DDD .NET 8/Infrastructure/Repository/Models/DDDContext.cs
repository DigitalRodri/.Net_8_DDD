using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Models
{
    public partial class DDDContext : DbContext
    {
        public DDDContext(DbContextOptions<DDDContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable(tb => tb.HasTrigger("UpdateDateTimeAccount"));
        }
    }
}
