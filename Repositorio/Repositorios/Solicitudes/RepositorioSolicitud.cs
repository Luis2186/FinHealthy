using Dominio.Solicitudes;
using Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Errores;
using Dominio.Familias;

namespace Repositorio.Repositorios.Solicitudes
{
    public class RepositorioSolicitud : IRepositorioSolicitud
    {
        private readonly ApplicationDbContext _context;

        public RepositorioSolicitud(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Resultado<bool>> AceptarSolicitud(int idSolicitud)
        {
            try
            {
                var resultado_solicitud = await ObtenerPorIdAsync(idSolicitud);

                if (resultado_solicitud.TieneErrores) return Resultado<bool>.Failure(resultado_solicitud.Errores);

                SolicitudUnionFamilia solicitud= resultado_solicitud.Valor;

                solicitud.EstadoSolicitudGrupo = new SUGF_Aceptada();
                solicitud.Aceptar();

                _context.SolcitudesUnionFamilia.Update(solicitud);
                var resultadoActualizado = await _context.SaveChangesAsync() == 1;

                if (!resultadoActualizado) return Resultado<bool>.Failure(ErroresCrud.ErrorDeActualizacion("Solicitud"));

                return Resultado<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("ACEPTAR_SOLICITUD", ex.Message));
            }
        }

        public async Task<Resultado<SolicitudUnionFamilia>> ActualizarAsync(SolicitudUnionFamilia model)
        {
            try
            {
                var solicitud = await ObtenerPorIdAsync(model.Id);

                if (solicitud.TieneErrores) return Resultado<SolicitudUnionFamilia>.Failure(solicitud.Errores);

                var resultadoValidacion = DataAnnotationsValidator.Validar(model);

                if (resultadoValidacion.TieneErrores) return resultadoValidacion;

                _context.SolcitudesUnionFamilia.Update(model);
                var resultadoActualizado = await _context.SaveChangesAsync() == 1;

                if (!resultadoActualizado) return Resultado<SolicitudUnionFamilia>.Failure(ErroresCrud.ErrorDeActualizacion("Solicitud"));

                return Resultado<SolicitudUnionFamilia>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<SolicitudUnionFamilia>.Failure(ErroresCrud.ErrorDeExcepcion("UPDATE", ex.Message));
            }
        }

        public async Task<Resultado<SolicitudUnionFamilia>> CrearAsync(SolicitudUnionFamilia model)
        {
            try
            {
                var resultadoValidacion = DataAnnotationsValidator.Validar(model);

                if (resultadoValidacion.TieneErrores) return resultadoValidacion;

                await _context.SolcitudesUnionFamilia.AddAsync(model);
                var resultadoCreado = await _context.SaveChangesAsync() == 1;

                if (!resultadoCreado) return Resultado<SolicitudUnionFamilia>.Failure(ErroresCrud.ErrorDeCreacion("Solicitud"));

                return Resultado<SolicitudUnionFamilia>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<SolicitudUnionFamilia>.Failure(ErroresCrud.ErrorDeExcepcion("CREATE", ex.Message));
            };
        }

        public async Task<Resultado<bool>> EliminarAsync(int id)
        {
            try
            {
                var solicitud = await ObtenerPorIdAsync(id);

                if (solicitud.TieneErrores) return Resultado<bool>.Failure(solicitud.Errores);

                _context.SolcitudesUnionFamilia.Remove(solicitud.Valor);
                var resultadoEliminado = await _context.SaveChangesAsync() == 1;

                if (!resultadoEliminado) return Resultado<bool>.Failure(ErroresCrud.ErrorDeEliminacion("Solicitud"));

                return Resultado<bool>.Success(resultadoEliminado);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("REMOVE", ex.Message));
            }
        }

        public async Task<Resultado<SolicitudUnionFamilia>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var solicitud = _context.SolcitudesUnionFamilia
                    .FirstOrDefault(f => f.Id == id);

                if (solicitud == null) return Resultado<SolicitudUnionFamilia>.Failure(ErroresCrud.ErrorBuscarPorId("Solicitud"));

                return Resultado<SolicitudUnionFamilia>.Success(solicitud);
            }
            catch (Exception ex)
            {
                return Resultado<SolicitudUnionFamilia>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_BY_ID", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<SolicitudUnionFamilia>>> ObtenerTodasPorAdministrador(string idAdministrador, string estado)
        {
            try
            {
                var solicitudesUnion = await _context.SolcitudesUnionFamilia
                    .Where(s => s.UsuarioAdministradorGrupoId == idAdministrador && s.Estado== estado)
                    .ToListAsync();

                return Resultado<IEnumerable<SolicitudUnionFamilia>>.Success(solicitudesUnion);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<SolicitudUnionFamilia>>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_ALL", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<SolicitudUnionFamilia>>> ObtenerTodosAsync()
        {
            try
            {
                var solicitudesUnion = await _context.SolcitudesUnionFamilia
                    .ToListAsync();

                return Resultado<IEnumerable<SolicitudUnionFamilia>>.Success(solicitudesUnion);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<SolicitudUnionFamilia>>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_ALL", ex.Message));
            }
        }

        public async Task<Resultado<bool>> RechazarSolicitud(int idSolicitud)
        {
            try
            {
                var resultado_solicitud = await ObtenerPorIdAsync(idSolicitud);

                if (resultado_solicitud.TieneErrores) return Resultado<bool>.Failure(resultado_solicitud.Errores);

                SolicitudUnionFamilia solicitud = resultado_solicitud.Valor;

                solicitud.EstadoSolicitudGrupo = new SUGF_Rechazada();
                solicitud.Rechazar();

                _context.SolcitudesUnionFamilia.Update(solicitud);
                var resultadoActualizado = await _context.SaveChangesAsync() == 1;

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
