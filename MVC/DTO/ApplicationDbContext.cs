using Microsoft.EntityFrameworkCore;

namespace DTO
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            // Eliminar cualquier código de conexión aquí, ya que la conexión será manejada externamente
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Cita> Citas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configura el modelo si es necesario
            // Ejemplo de configuración adicional:
            // modelBuilder.Entity<User>().HasIndex(u => u.CorreoElectronico).IsUnique();
        }
    }
}
