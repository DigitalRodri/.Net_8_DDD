using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Infraestructure.Repository.Models
{
    public partial class DDDContext : DbContext
    {
        public DDDContext(DbContextOptions<DDDContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
    }
}
