using Dominio.Usuarios;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


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





    }
}
