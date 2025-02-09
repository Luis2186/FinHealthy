using Dominio;
using Dominio.Abstracciones;
using Dominio.Notificaciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.Notificaciones
{
    public class RepositorioNotificacion : IRepositorioNotificacion
    {
        private readonly ApplicationDbContext _dbContext;

        public RepositorioNotificacion(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Resultado<Notificacion>> BuscarNotificacion(int notificacionId)
        {
            try
            {
                Notificacion notificacionActualizar = await _dbContext.Notificaciones.FindAsync(notificacionId);

                if (notificacionActualizar == null) return Resultado<Notificacion>.Failure(ErroresNotificacion.NotificacionNoEncontrada);

                return Resultado<Notificacion>.Success(notificacionActualizar);
            }
            catch (Exception ex)
            {
                return Resultado<Notificacion>.Failure(new Error("ErrorException.BuscarNotificacion", $"Error al buscar la notificación: {ex.Message}"));
            }
        }

        public async Task<Resultado<bool>> EliminarNotificacion(int notificacionId)
        {
            try
            {
                var notificacionActualizar = await BuscarNotificacion(notificacionId);

                if (notificacionActualizar.TieneErrores) return Resultado<bool>.Failure(notificacionActualizar.Errores);

                var notificacion = notificacionActualizar.Valor;

                bool notificacionDesactivada = notificacion.DesactivarNotificacion();

                _dbContext.Notificaciones.Update(notificacion);

                int filasAfectadas = await _dbContext.SaveChangesAsync();

                if (filasAfectadas >= 1) return Resultado<bool>.Success(notificacionDesactivada);

                return Resultado<bool>.Failure(ErroresNotificacion.NotificacionNoSePudoEliminar);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(new Error("ErrorException", $"Error al eliminar la notificación: {ex.Message}"));
            }
        }

        public async Task<Resultado<bool>> EnviarNotificacion(Notificacion notificacion)
        {
            try
            {
                await _dbContext.Notificaciones.AddAsync(notificacion);
                int filasAfectadas = await _dbContext.SaveChangesAsync();

                if (filasAfectadas >= 1) return Resultado<bool>.Success(true);
                
                return Resultado<bool>.Failure(ErroresNotificacion.ErrorEnvio);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(new Error("ErrorException", $"Error al enviar la notificación: {ex.Message}"));
            }
        }

        public async Task<Resultado<bool>> MarcarComoLeida(int notificacionId)
        {
            try
            {
                var notificacionActualizar = await BuscarNotificacion(notificacionId);

                if (notificacionActualizar.TieneErrores) return Resultado<bool>.Failure(notificacionActualizar.Errores);

                var notificacion = notificacionActualizar.Valor;

                bool notificacionLeida = notificacion.NotificacionLeida();

                _dbContext.Notificaciones.Update(notificacion);

                int filasAfectadas = await _dbContext.SaveChangesAsync();

                if (filasAfectadas >= 1) return Resultado<bool>.Success(notificacionLeida);

                return Resultado<bool>.Failure(ErroresNotificacion.NotificacionNoSePudoLeer);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(new Error("ErrorException", $"Error al leer la notificación: {ex.Message}"));
            }
        }

        public async Task<Resultado<IEnumerable<Notificacion>>> ObtenerNotificacionesEmitidas(string usuarioEmisorId)
        {
            return _dbContext.Notificaciones
                .Include(noti => noti.UsuarioEmisor)
                .Where(noti => noti.UsuarioEmisor.Id == usuarioEmisorId && noti.Activa == true).ToList();
        } 

        public async Task<Resultado<IEnumerable<Notificacion>>> ObtenerNotificacionesRecibidas(string usuarioReceptorId)
        {
            return _dbContext.Notificaciones
                .Include(noti => noti.UsuarioReceptor)
                .Where(noti => noti.UsuarioReceptor.Id == usuarioReceptorId && noti.Activa == true).ToList();
        }
    }
}
