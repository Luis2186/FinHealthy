using Dominio.Documentos;
using Dominio.Gastos;
using Dominio.Grupos;
using Dominio.Notificaciones;
using Dominio.Solicitudes;
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
            ConfigurarBuilderNotificacion(builder);
            ConfigurarBuilderSolicitudesGrupo(builder);
            ConfigurarBuilderGrupo(builder);
            ConfigurarBuilderCategoria(builder);
            ConfigurarBuilderSubCategoria(builder);
            ConfigurarBuilderMetodoDePago(builder);
            ConfigurarBuilderMoneda(builder);
            ConfigurarBuilderTipoDeDocumento(builder);

            builder.HasDefaultSchema("identity");
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
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired(); // Evita eliminar notificaciones al borrar el usuario

                // Relación con UsuarioReceptor (muchas notificaciones pueden tener un mismo receptor)
                entity.HasOne(n => n.UsuarioReceptor)
                      .WithMany(u => u.Notificaciones) // Asumiendo que Usuario tiene una colección de notificaciones recibidas
                      .HasForeignKey("UsuarioReceptorId")
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired(); // Borra notificaciones al eliminar el usuario receptor

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

        protected private void ConfigurarBuilderSolicitudesGrupo(ModelBuilder builder)
        {
            // Configuración de la tabla Notificaciones
            builder.Entity<SolicitudUnionGrupo>(entity =>
            {
                // Configuración de clave primaria
                entity.HasKey(n => n.Id);
                
                // Definir cómo se almacena el estado en la base de datos

                    entity.Property(p => p.EstadoSolicitudGrupo)
                    .HasConversion(
                        v => v.GetType().Name,   // Convertir el estado a un nombre de tipo
                        v => CrearEstado(v)      // Reconstruir el objeto de estado desde el nombre
                    );

                // Relación con UsuarioSolicitante (muchas solicitudes pueden tener un mismo solicitante)
                entity.HasOne(n => n.UsuarioSolicitante)
                      .WithMany() // Sin colección relacionada explícita
                      .HasForeignKey(n => n.UsuarioSolicitanteId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired();

                // Relación con UsuarioAdministradorGrupo (muchas solicitudes pueden tener un mismo solicitante)
                entity.HasOne(n => n.UsuarioAdministradorGrupo)
                      .WithMany() // Sin colección relacionada explícita
                      .HasForeignKey(n => n.UsuarioAdministradorGrupoId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired();
                    
                entity.Property(n => n.Estado)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(n => n.FechaDeEnvio)
                      .HasDefaultValueSql("GETUTCDATE()"); // Valor por defecto en SQL Server

                entity.Property(n => n.FechaDeRespuesta)
                     .IsRequired(false);

                entity.Property(n => n.CodigoDeSeguridad)
                .HasMaxLength(150);
            });
        }

        private ISolicitudUnionGrupo CrearEstado(string estado)
        {
            return estado switch
            {
                "SUGF_Pendiente" => new SUG_Pendiente(),
                "SUGF_Aceptada" => new SUG_Aceptada(),
                "SUGF_Rechazada" => new SUG_Rechazada(),
                _ => throw new ArgumentException("Estado desconocido")
            };
        }

        protected private void ConfigurarBuilderGrupo(ModelBuilder builder)
        {
            builder.Entity<Grupo>(entity =>
            {
                // Configuración de clave primaria
                entity.HasKey(g => g.Id);

                // Relación con UsuarioAdministrador (uno a uno)
                entity.HasOne(g => g.UsuarioAdministrador) // Relación con Usuario
                      .WithOne() // Relación uno a uno
                      .HasForeignKey<Grupo>(g => g.UsuarioAdministradorId) // Clave foránea explícita
                      .OnDelete(DeleteBehavior.Restrict); // No permite eliminar al administrador si el grupo está activo

                // Relación con UsuarioAdministrador (uno a uno)
                entity.HasMany(g => g.SubCategorias) // Relación con Usuario
                      .WithOne() // Relación uno a uno
                      .HasForeignKey(g => g.GrupoGastoId) // Clave foránea explícita
                      .OnDelete(DeleteBehavior.Restrict); 

                // Configuración de propiedades adicionales
                entity.Property(g => g.Nombre)
                      .HasMaxLength(100);

                entity.Property(g => g.Descripcion)
                      .HasMaxLength(250);

                entity.HasIndex(user => user.Nombre).IsUnique();

                entity.Property(g => g.FechaDeCreacion)
                      .HasDefaultValueSql("GETUTCDATE()");
            });
        
        }

        protected private void ConfigurarBuilderUsuario(ModelBuilder builder)
        {
            builder.Entity<Usuario>().Property(user => user.Nombre).HasMaxLength(60).IsRequired();
            builder.Entity<Usuario>().Property(user => user.Apellido).HasMaxLength(60).IsRequired();
            builder.Entity<Usuario>().Property(user => user.FechaDeNacimiento).IsRequired();
            builder.Entity<Usuario>().Property(user => user.FechaDeRegistro).IsRequired();
            builder.Entity<Usuario>().Property(user => user.Activo).HasDefaultValue(true);
            builder.Entity<Usuario>().HasIndex(user => user.Email).IsUnique();

            builder.Entity<Usuario>(entity =>
            {
                // Clave primaria
                entity.HasKey(m => m.Id);

                // Relación con Grupo
                entity.HasOne(m => m.GrupoDeGastos)
                      .WithMany(g => g.MiembrosGrupoGasto) // Un Grupo tiene múltiples usuarios
                      .HasForeignKey(m => m.GrupoDeGastosId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired(false);

                // Configuración de propiedades
                entity.Property(m => m.FechaDeUnion)
                .IsRequired(false);

                entity.Property(m => m.Activo)
                      .IsRequired();
            });
        }

        protected private void ConfigurarBuilderCategoria(ModelBuilder builder)
        {
            builder.Entity<Categoria>(entity =>
            {
                // Clave primaria
                entity.HasKey(c => c.Id);

                // Relación con UsuarioAdministrador (uno a uno)
                entity.HasMany(g => g.SubCategorias) // Relación con Usuario
                      .WithOne() // Relación uno a uno
                      .HasForeignKey(g => g.CategoriaId) // Clave foránea explícita
                      .OnDelete(DeleteBehavior.Restrict);

                // Configuración de propiedades
                entity.Property(c => c.Nombre)
                .IsRequired(true);

                entity.HasIndex(c => c.Nombre).IsUnique();
            });
        }
        protected private void ConfigurarBuilderSubCategoria(ModelBuilder builder)
        {
            builder.Entity<SubCategoria>(entity =>
            {
                // Clave primaria
                entity.HasKey(c => c.Id);

                // Relación entre Subcategoría y Grupo (una subcategoría pertenece a un grupo)
                    entity
                    .HasOne(s => s.GrupoGasto)
                    .WithMany(f => f.SubCategorias)
                    .HasForeignKey(s => s.GrupoGastoId);

                // Relación entre Subcategoría y Categoría Principal (una subcategoría pertenece a una categoría principal)
                entity
                     .HasOne(s => s.Categoria)
                    .WithMany(c => c.SubCategorias)
                    .HasForeignKey(s => s.CategoriaId);

                entity.HasIndex(s => s.Nombre).IsUnique();
            });
        }
        protected private void ConfigurarBuilderMoneda(ModelBuilder builder)
        {
            builder.Entity<Moneda>(entity =>
            {
                // Clave primaria
                entity.HasKey(m => m.Codigo);

                // Configuración de propiedades
                entity.Property(c => c.Nombre)
                .IsRequired(true);

                entity.Property(c => c.Simbolo)
                .IsRequired(true);

                entity.Property(c => c.Pais)
                .IsRequired(true);

                entity.Property(c => c.TipoDeCambio)
                .IsRequired(true);

                entity.HasIndex(c => new { c.Codigo,c.Nombre } ).IsUnique();
            });
        }

        protected private void ConfigurarBuilderTipoDeDocumento(ModelBuilder builder)
        {
            builder.Entity<TipoDeDocumento>(entity =>
            {
                // Clave primaria
                entity.HasKey(c => c.Id);

                // Configuración de propiedades
                entity.Property(c => c.Nombre)
                .IsRequired(true);

                entity.HasIndex(c => c.Nombre).IsUnique();
            });
        }

        protected private void ConfigurarBuilderMetodoDePago(ModelBuilder builder)
        {
            builder.Entity<MetodoDePago>(entity =>
            {
                // Clave primaria
                entity.HasKey(c => c.Id);

                // Configuración de propiedades
                entity.Property(c => c.Nombre)
                .IsRequired(true);

                entity.HasIndex(c => c.Nombre).IsUnique();
            });
        }

        // DbSet para las notificaciones
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<SolicitudUnionGrupo> SolcitudesUnionGrupo { get; set; }
        public DbSet<Grupo> GruposDeGasto { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<SubCategoria> SubCategorias { get; set; }
        public DbSet<Moneda> Monedas { get; set; }
        public DbSet<TipoDeDocumento> TipoDeDocumentos { get; set; }
        public DbSet<MetodoDePago> MetodosDePago { get; set; }

    }
}
