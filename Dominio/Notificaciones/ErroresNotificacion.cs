using Dominio.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Notificaciones
{
    public class ErroresNotificacion
    {
        public static readonly Error ErrorEnvio = new("Notificacion.Enviar", "Ah ocurrido un error al enviar la notificacion");
        public static readonly Error NotificacionNoEncontrada = new("Notificacion.Buscar", "La notificación no se ha encontrado con el ID proporcionado.");
        public static readonly Error NotificacionNoSePudoLeer = new("Notificacion.Actualizar", "La notificación no se ha podido actualizar a leida.");
        public static readonly Error NotificacionNoSePudoEliminar = new("Notificacion.Desactivar", "La notificación no se ha podido eliminar.");
    }

}
