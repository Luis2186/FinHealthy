using Dominio.Notificaciones;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servicio.Notificaciones.NotificacionesDTO;

namespace Servicio.Notificaciones
{
    public interface IServicioNotificacion
    {
        public Task<Resultado<NotificacionDTO>> EnviarNotificacion(NotificacionCreacionDTO notificacion);
        public Task<Resultado<IEnumerable<NotificacionDTO>>> ObtenerNotificacionesEmitidas(string usuarioEmisorId);
        public Task<Resultado<IEnumerable<NotificacionDTO>>> ObtenerNotificacionesRecibidas(string usuarioReceptorId);
        public Task<Resultado<bool>> MarcarComoLeida(int notificacionId);
        public Task<Resultado<NotificacionDTO>> BuscarNotificacion(int notificacionId);
        public Task<Resultado<bool>> EliminarNotificacion(int notificacionId);
    }
}
