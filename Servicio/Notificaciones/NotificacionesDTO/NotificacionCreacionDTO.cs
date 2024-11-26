using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.Notificaciones.NotificacionesDTO
{
    public class NotificacionCreacionDTO
    {

        [Required(ErrorMessage = "El Id de usuario que emite la notificacion es requerido")]
        public string UsuarioEmisorId { get; set; }
        [Required(ErrorMessage = "El Id de  usuario que recibe la notificacion es requerido")]
        public string UsuarioReceptorId { get; set; }
        [Required(ErrorMessage = "El mensaje de la notificacion no puede estar vacio")]
        [StringLength(100, ErrorMessage = "El mensaje de la notificacion no puede tener mas de 100 caracteres")]
        public string Mensaje { get; set; } = string.Empty;
    }
}
