﻿using Dominio.Documentos;
using Dominio.Familias;
using Dominio.Gastos;
using Dominio.Notificaciones;
using Dominio.Solicitudes;
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
            ConfigurarBuilderSolicitudesGrupoFamiliar(builder);
            ConfigurarBuilderGrupoFamiliar(builder);
            ConfigurarBuilderMiembroFamiliar(builder);
            ConfigurarBuilderCategoria(builder);
            ConfigurarBuilderMetodoDePago(builder);
            ConfigurarBuilderMoneda(builder);
            ConfigurarBuilderTipoDeDocumento(builder);

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

        protected private void ConfigurarBuilderSolicitudesGrupoFamiliar(ModelBuilder builder)
        {
            // Configuración de la tabla Notificaciones
            builder.Entity<SolicitudUnionFamilia>(entity =>
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

        private ISolicitudUnionGrupoFamiliar CrearEstado(string estado)
        {
            return estado switch
            {
                "SUGF_Pendiente" => new SUGF_Pendiente(),
                "SUGF_Aceptada" => new SUGF_Aceptada(),
                "SUGF_Rechazada" => new SUGF_Rechazada(),
                _ => throw new ArgumentException("Estado desconocido")
            };
        }

        protected private void ConfigurarBuilderGrupoFamiliar(ModelBuilder builder)
        {
            builder.Entity<Familia>(entity =>
            {
                // Configuración de clave primaria
                entity.HasKey(g => g.Id);

                // Relación con UsuarioAdministrador (uno a uno)
                entity.HasOne(g => g.UsuarioAdministrador) // Relación con Usuario
                      .WithOne() // Relación uno a uno
                      .HasForeignKey<Familia>(g => g.UsuarioAdministradorId) // Clave foránea explícita
                      .OnDelete(DeleteBehavior.Restrict); // No permite eliminar al administrador si el grupo está activo

                // Configuración de propiedades adicionales
                entity.Property(g => g.Apellido)
                      .HasMaxLength(100);

                entity.Property(g => g.Descripcion)
                      .HasMaxLength(250);

                entity.HasIndex(user => user.Apellido).IsUnique();

                entity.Property(g => g.FechaDeCreacion)
                      .HasDefaultValueSql("GETUTCDATE()");
            });
        
        }

        protected private void ConfigurarBuilderMiembroFamiliar(ModelBuilder builder)
        {
            builder.Entity<MiembroFamilia>(entity =>
            {
                // Clave primaria
                entity.HasKey(m => m.Id);

                // Relación con Usuario
                entity.HasOne(m => m.Usuario)
                      .WithMany() // Un Usuario puede estar asociado a varios MiembroFamilia
                      .HasForeignKey(m => m.UsuarioId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired(); // Evita eliminar Usuario si tiene miembros asociados

                // Relación con GrupoFamiliar
                entity.HasOne(m => m.GrupoFamiliar)
                      .WithMany(g => g.Miembros) // Un GrupoFamiliar tiene múltiples MiembroFamilia
                      .HasForeignKey(m => m.GrupoFamiliarId)
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
            
                // Configuración de propiedades
                entity.Property(c => c.Nombre)
                .IsRequired(true);

                entity.HasIndex(c => c.Nombre).IsUnique();
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
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<SolicitudUnionFamilia> SolcitudesUnionFamilia { get; set; }
        public DbSet<Familia> Familias { get; set; }
        public DbSet<MiembroFamilia> MiembrosFamiliares { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Moneda> Monedas { get; set; }
        public DbSet<TipoDeDocumento> TipoDeDocumentos { get; set; }
        public DbSet<MetodoDePago> MetodosDePago { get; set; }

    }
}
