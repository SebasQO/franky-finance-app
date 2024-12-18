using Microsoft.EntityFrameworkCore;

namespace FrankyFinance.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Grupos { get; set; }
        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<ExpenseSplit> ExpenseSplits { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Clave primaria compuesta en GroupUser
            modelBuilder.Entity<GroupUser>()
                .HasKey(gu => new { gu.GroupId, gu.UserId });

            modelBuilder.Entity<GroupUser>()
                .HasOne(gu => gu.Group)
                .WithMany(g => g.GroupUsers)
                .HasForeignKey(gu => gu.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupUser>()
                .HasOne(gu => gu.User)
                .WithMany(u => u.GroupUsers)
                .HasForeignKey(gu => gu.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración para la tabla Pagos
            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Group)
                .WithMany(g => g.Pagos)
                .HasForeignKey(p => p.GroupId)
                .OnDelete(DeleteBehavior.Cascade); // Si se borra un grupo, se borran los pagos asociados.

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Pagador)
                .WithMany()
                .HasForeignKey(p => p.PagadorId)
                .OnDelete(DeleteBehavior.Restrict); // Evita conflicto en cascada para Pagador.

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Receptor)
                .WithMany()
                .HasForeignKey(p => p.ReceptorId)
                .OnDelete(DeleteBehavior.Restrict); // Evita conflicto en cascada para Receptor.

            // Configuración para Gasto
            modelBuilder.Entity<Gasto>()
                .HasOne(g => g.Group)
                .WithMany(gr => gr.Gastos)
                .HasForeignKey(g => g.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
