using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.Notificaciones.NotificacionesDTO
{
    public class NotificacionDTO : NotificacionCreacionDTO
    {
        public int Id { get; set; }
        public bool Leida { get; set; } = false;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaDeLectura { get; set; }
        public bool Activa { get; set; }
    }
}
