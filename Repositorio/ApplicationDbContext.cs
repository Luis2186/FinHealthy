using Dominio.Notificaciones;
using Dominio.Usuarios;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;


namespace Repositorio
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            ConfigurarBuilderUsuario(builder);
            ConfigurarBuilderNotificacion(builder);

            builder.HasDefaultSchema("identity");
        }

        protected private void ConfigurarBuilderUsuario(ModelBuilder builder)
        {
            builder.Entity<Usuario>().Property(user => user.Nombre).HasMaxLength(60).IsRequired();
            builder.Entity<Usuario>().Property(user => user.Apellido).HasMaxLength(60).IsRequired();
            builder.Entity<Usuario>().Property(user => user.FechaDeNacimiento).IsRequired();
            builder.Entity<Usuario>().Property(user => user.FechaDeRegistro).IsRequired();
            builder.Entity<Usuario>().Property(user => user.Activo).HasDefaultValue(true);
            builder.Entity<Usuario>().HasIndex(user => user.Email).IsUnique();
        }

        protected private void ConfigurarBuilderNotificacion(ModelBuilder builder)
        {
            // Configuración de la tabla Notificaciones
            builder.Entity<Notificacion>(entity =>
            {
                // Configuración de clave primaria
                entity.HasKey(n => n.Id);

                // Relación con UsuarioEmisor (muchas notificaciones pueden tener un mismo emisor)
                entity.HasOne(n => n.UsuarioEmisor)
                      .WithMany() // No mapeamos una colección en Usuario para simplificar
                      .HasForeignKey("UsuarioEmisorId") // Propiedad FK implícita
                      .OnDelete(DeleteBehavior.Restrict); // Evita eliminar notificaciones al borrar el usuario

                // Relación con UsuarioReceptor (muchas notificaciones pueden tener un mismo receptor)
                entity.HasOne(n => n.UsuarioReceptor)
                      .WithMany(u => u.Notificaciones) // Asumiendo que Usuario tiene una colección de notificaciones recibidas
                      .HasForeignKey("UsuarioReceptorId")
                      .OnDelete(DeleteBehavior.Cascade); // Borra notificaciones al eliminar el usuario receptor

                // Configuración del campo Mensaje
                entity.Property(n => n.Mensaje)
                      .IsRequired()
                      .HasMaxLength(150);

                // Configuración de FechaCreacion
                entity.Property(n => n.FechaCreacion)
                      .HasDefaultValueSql("GETUTCDATE()"); // Valor por defecto en SQL Server

                // Configuración de Leida
                entity.Property(n => n.Leida)
                      .HasDefaultValue(false); // Valor por defecto al crear

                // Configuración de FechaDeLectura
                entity.Property(n => n.FechaDeLectura)
                      .HasDefaultValueSql("GETUTCDATE()"); // Inicializa por defecto al crear
            });
        }

        // DbSet para las notificaciones
        public DbSet<Notificacion> Notificaciones { get; set; }


    }
}
