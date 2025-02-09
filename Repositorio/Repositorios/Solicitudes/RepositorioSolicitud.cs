using Dominio.Solicitudes;
using Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Errores;

namespace Repositorio.Repositorios.Solicitudes
{
    public class RepositorioSolicitud : IRepositorioSolicitud
    {
        private readonly ApplicationDbContext _dbContext;

        public RepositorioSolicitud(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Resultado<bool>> AceptarSolicitud(int idSolicitud)
        {
            try
            {
                var resultado_solicitud = await ObtenerPorIdAsync(idSolicitud);

                if (resultado_solicitud.TieneErrores) return Resultado<bool>.Failure(resultado_solicitud.Errores);

                SolicitudUnionGrupo solicitud= resultado_solicitud.Valor;

                solicitud.EstadoSolicitudGrupo = new SUG_Aceptada();
                solicitud.Aceptar();

                _dbContext.SolcitudesUnionGrupo.Update(solicitud);
                var resultadoActualizado = await _dbContext.SaveChangesAsync() >= 1;

                if (!resultadoActualizado) return Resultado<bool>.Failure(ErroresCrud.ErrorDeActualizacion("Solicitud"));

                return Resultado<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("ACEPTAR_SOLICITUD", ex.Message));
            }
        }

        public async Task<Resultado<SolicitudUnionGrupo>> ActualizarAsync(SolicitudUnionGrupo model)
        {
            try
            {
                var solicitud = await ObtenerPorIdAsync(model.Id);

                if (solicitud.TieneErrores) return Resultado<SolicitudUnionGrupo>.Failure(solicitud.Errores);

                var resultadoValidacion = DataAnnotationsValidator.Validar(model);

                if (resultadoValidacion.TieneErrores) return resultadoValidacion;

                _dbContext.SolcitudesUnionGrupo.Update(model);
                var resultadoActualizado = await _dbContext.SaveChangesAsync() >= 1;

                if (!resultadoActualizado) return Resultado<SolicitudUnionGrupo>.Failure(ErroresCrud.ErrorDeActualizacion("Solicitud"));

                return Resultado<SolicitudUnionGrupo>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<SolicitudUnionGrupo>.Failure(ErroresCrud.ErrorDeExcepcion("UPDATE", ex.Message));
            }
        }

        public async Task<Resultado<SolicitudUnionGrupo>> CrearAsync(SolicitudUnionGrupo model)
        {
            try
            {
                var resultadoValidacion = DataAnnotationsValidator.Validar(model);

                if (resultadoValidacion.TieneErrores) return resultadoValidacion;

                await _dbContext.SolcitudesUnionGrupo.AddAsync(model);
                var resultadoCreado = await _dbContext.SaveChangesAsync() >= 1;

                if (!resultadoCreado) return Resultado<SolicitudUnionGrupo>.Failure(ErroresCrud.ErrorDeCreacion("Solicitud"));

                return Resultado<SolicitudUnionGrupo>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<SolicitudUnionGrupo>.Failure(ErroresCrud.ErrorDeExcepcion("CREATE", ex.Message));
            };
        }

        public async Task<Resultado<bool>> EliminarAsync(int id)
        {
            try
            {
                var solicitud = await ObtenerPorIdAsync(id);

                if (solicitud.TieneErrores) return Resultado<bool>.Failure(solicitud.Errores);

                _dbContext.SolcitudesUnionGrupo.Remove(solicitud.Valor);
                var resultadoEliminado = await _dbContext.SaveChangesAsync() >= 1;

                if (!resultadoEliminado) return Resultado<bool>.Failure(ErroresCrud.ErrorDeEliminacion("Solicitud"));

                return Resultado<bool>.Success(resultadoEliminado);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("REMOVE", ex.Message));
            }
        }

        public async Task<Resultado<SolicitudUnionGrupo>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var solicitud = _dbContext.SolcitudesUnionGrupo
                    .FirstOrDefault(f => f.Id == id);

                if (solicitud == null) return Resultado<SolicitudUnionGrupo>.Failure(ErroresCrud.ErrorBuscarPorId("Solicitud"));

                return Resultado<SolicitudUnionGrupo>.Success(solicitud);
            }
            catch (Exception ex)
            {
                return Resultado<SolicitudUnionGrupo>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_BY_ID", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<SolicitudUnionGrupo>>> ObtenerTodasPorAdministrador(string idAdministrador, string estado)
        {
            try
            {
                var solicitudesUnion = await _dbContext.SolcitudesUnionGrupo
                    .Where(s => s.UsuarioAdministradorGrupoId == idAdministrador && s.Estado== estado)
                    .ToListAsync();

                return Resultado<IEnumerable<SolicitudUnionGrupo>>.Success(solicitudesUnion);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<SolicitudUnionGrupo>>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_ALL", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<SolicitudUnionGrupo>>> ObtenerTodosAsync()
        {
            try
            {
                var solicitudesUnion = await _dbContext.SolcitudesUnionGrupo
                    .ToListAsync();

                return Resultado<IEnumerable<SolicitudUnionGrupo>>.Success(solicitudesUnion);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<SolicitudUnionGrupo>>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_ALL", ex.Message));
            }
        }

        public async Task<Resultado<bool>> RechazarSolicitud(int idSolicitud)
        {
            try
            {
                var resultado_solicitud = await ObtenerPorIdAsync(idSolicitud);

                if (resultado_solicitud.TieneErrores) return Resultado<bool>.Failure(resultado_solicitud.Errores);

                SolicitudUnionGrupo solicitud = resultado_solicitud.Valor;

                solicitud.EstadoSolicitudGrupo = new SUG_Rechazada();
                solicitud.Rechazar();

                _dbContext.SolcitudesUnionGrupo.Update(solicitud);
                var resultadoActualizado = await _dbContext.SaveChangesAsync() >= 1;

                if (!resultadoActualizado) return Resultado<bool>.Failure(ErroresCrud.ErrorDeActualizacion("Solicitud"));

                return Resultado<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("RECHAZAR_SOLICITUD", ex.Message));
            }
        }
    }
}
