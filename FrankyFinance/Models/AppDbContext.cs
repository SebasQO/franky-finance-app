using Microsoft.EntityFrameworkCore;

namespace FrankyFinance.Models
{
    // Contexto de base de datos principal que gestiona las entidades y sus relaciones
    public class AppDbContext : DbContext
    {
        // Constructor que recibe las opciones de configuración del DbContext
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets: Representan las tablas en la base de datos
        public DbSet<User> Users { get; set; }            // Tabla de usuarios
        public DbSet<Group> Grupos { get; set; }          // Tabla de grupos
        public DbSet<Gasto> Gastos { get; set; }          // Tabla de gastos
        public DbSet<GroupUser> GroupUsers { get; set; }  // Tabla intermedia de usuarios y grupos
        public DbSet<ExpenseSplit> ExpenseSplits { get; set; }  // Tabla de divisiones de gastos
        public DbSet<Pago> Pagos { get; set; }            // Tabla de pagos

        // Configuración del modelo y relaciones entre las entidades
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la clave primaria compuesta para la tabla GroupUser
            modelBuilder.Entity<GroupUser>()
                .HasKey(gu => new { gu.GroupId, gu.UserId });

            // Relación: Un GroupUser pertenece a un Group (muchos GroupUsers en un Group)
            modelBuilder.Entity<GroupUser>()
                .HasOne(gu => gu.Group)
                .WithMany(g => g.GroupUsers)
                .HasForeignKey(gu => gu.GroupId);

            // Relación: Un GroupUser pertenece a un User (muchos GroupUsers en un User)
            modelBuilder.Entity<GroupUser>()
                .HasOne(gu => gu.User)
                .WithMany(u => u.GroupUsers)
                .HasForeignKey(gu => gu.UserId);

            // Relación: Un Pago tiene un Pagador (usuario que paga)
            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Pagador)
                .WithMany()
                .HasForeignKey(p => p.PagadorId)
                .OnDelete(DeleteBehavior.Restrict); // Restricción para evitar borrado en cascada

            // Relación: Un Pago tiene un Receptor (usuario que recibe el pago)
            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Receptor)
                .WithMany()
                .HasForeignKey(p => p.ReceptorId)
                .OnDelete(DeleteBehavior.Restrict); // Restricción para evitar borrado en cascada
        }
    }
}
