using Microsoft.EntityFrameworkCore;

namespace FrankyFinance.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Grupos { get; set; }
        public DbSet<Gasto> Gastos { get; set; }

    }
}
