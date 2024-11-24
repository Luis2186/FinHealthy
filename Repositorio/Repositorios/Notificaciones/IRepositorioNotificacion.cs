using Dominio;
using Dominio.Notificaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.Notificaciones
{
    public interface IRepositorioNotificacion 
    {
        public Task<Resultado<bool>> EnviarNotificacion(Notificacion notificacion);
        public Task<Resultado<IEnumerable<Notificacion>>> ObtenerNotificacionesEmitidas(string usuarioEmisorId);
        public Task<Resultado<IEnumerable<Notificacion>>> ObtenerNotificacionesRecibidas(string usuarioReceptorId);
        public Task<Resultado<bool>> MarcarComoLeida(int notificacionId);
        public Task<Resultado<Notificacion>> BuscarNotificacion(int notificacionId);
        public Task<Resultado<bool>> EliminarNotificacion(int notificacionId);
    }
}
