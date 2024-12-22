using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Notificaciones
{
    public class Notificacion
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="El usuario que emite la notificacion es requerido")]
        public Usuario? UsuarioEmisor { get; set; }
        [Required(ErrorMessage = "El usuario que recibe la notificacion es requerido")]
        public Usuario? UsuarioReceptor { get; set; }
        [Required(ErrorMessage = "El mensaje de la notificacion no puede estar vacio")]
        [StringLength(100, ErrorMessage = "El mensaje de la notificacion no puede tener mas de 100 caracteres")]
        public string? Mensaje { get; set; } = string.Empty;
        public bool Leida { get; set; } = false;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaDeLectura { get; set; } = DateTime.UtcNow;
        public bool Activa { get; set; }

        public Notificacion() { }
        public Notificacion(Usuario UsuarioEmisor, Usuario UsuarioReceptor,string mensaje)
        {
            this.UsuarioEmisor = UsuarioEmisor;
            this.UsuarioReceptor = UsuarioReceptor;
            this.FechaCreacion = DateTime.UtcNow;
            this.Mensaje = mensaje;
        }

        public bool NotificacionLeida()
        {
            this.Leida = true;
            this.FechaDeLectura = DateTime.UtcNow;

            return this.Leida;
        }

        public bool DesactivarNotificacion()
        {
            return this.Activa = false;
        }

    }
}
