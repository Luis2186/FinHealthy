using Dominio.Solicitudes;
using Repositorio.Repositorios.Validacion;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Dominio.Errores;
using Dominio;

namespace Repositorio.Repositorios.Solicitudes
{
    public class RepositorioSolicitud : RepositorioCRUD<SolicitudUnionGrupo>, IRepositorioSolicitud
    {
        public RepositorioSolicitud(ApplicationDbContext context, IValidacion<SolicitudUnionGrupo> validacion)
            : base(context, validacion)
        {
        }

        public async Task<Resultado<bool>> AceptarSolicitud(int idSolicitud, CancellationToken cancellationToken)
        {
            var resultado_solicitud = await ObtenerPorIdAsync(idSolicitud, cancellationToken);
            if (resultado_solicitud.TieneErrores) return Resultado<bool>.Failure(resultado_solicitud.Errores);
            var solicitud = resultado_solicitud.Valor;
            solicitud.EstadoSolicitudGrupo = new SUG_Aceptada();
            solicitud.Aceptar();
            _dbContext.SolcitudesUnionGrupo.Update(solicitud);
            var resultadoActualizado = await _dbContext.SaveChangesAsync(cancellationToken) >= 1;
            if (!resultadoActualizado) return Resultado<bool>.Failure(ErroresCrud.ErrorDeActualizacion("Solicitud"));
            return Resultado<bool>.Success(true);
        }

        public async Task<Resultado<bool>> RechazarSolicitud(int idSolicitud, CancellationToken cancellationToken)
        {
            var resultado_solicitud = await ObtenerPorIdAsync(idSolicitud, cancellationToken);
            if (resultado_solicitud.TieneErrores) return Resultado<bool>.Failure(resultado_solicitud.Errores);
            var solicitud = resultado_solicitud.Valor;
            solicitud.EstadoSolicitudGrupo = new SUG_Rechazada();
            solicitud.Rechazar();
            _dbContext.SolcitudesUnionGrupo.Update(solicitud);
            var resultadoActualizado = await _dbContext.SaveChangesAsync(cancellationToken) >= 1;
            if (!resultadoActualizado) return Resultado<bool>.Failure(ErroresCrud.ErrorDeActualizacion("Solicitud"));
            return Resultado<bool>.Success(true);
        }

        public async Task<Resultado<IEnumerable<SolicitudUnionGrupo>>> ObtenerTodasPorAdministrador(string idAdministrador, string estado, CancellationToken cancellationToken)
        {
            var solicitudesUnion = await _dbContext.SolcitudesUnionGrupo
                .Where(s => s.UsuarioAdministradorGrupoId == idAdministrador && s.Estado == estado)
                .ToListAsync(cancellationToken);
            return Resultado<IEnumerable<SolicitudUnionGrupo>>.Success(solicitudesUnion);
        }
    }
}
