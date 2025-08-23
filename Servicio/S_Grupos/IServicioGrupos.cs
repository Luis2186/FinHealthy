using Dominio;
using Servicio.DTOS.GruposDTO;
using Servicio.DTOS.SolicitudesDTO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio.S_Grupos
{
    public interface IServicioGrupos
    {
        Task<Resultado<GrupoDTO>> ObtenerGrupoPorId(int id, CancellationToken cancellationToken);
        Task<Resultado<IEnumerable<GrupoDTO>>> ObtenerTodosLosGrupos(CancellationToken cancellationToken);
        Task<Resultado<IEnumerable<GrupoDTO>>> ObtenerTodosLosGruposPorUsuario(string idUsuario, CancellationToken cancellationToken);
        Task<Resultado<GrupoDTO>> CrearGrupo(CrearGrupoDTO grupoCreacionDTO, CancellationToken cancellationToken);
        Task<Resultado<GrupoDTO>> ActualizarGrupo(int grupoId, ActualizarGrupoDTO grupoActualizacionDTO, CancellationToken cancellationToken);
        Task<Resultado<bool>> EliminarGrupo(int id, CancellationToken cancellationToken);
        Task<Resultado<bool>> AceptarSolicitudIngresoAGrupo(int idSolicitud, CancellationToken cancellationToken);
        Task<Resultado<bool>> RechazarSolicitudIngresoAGrupo(int idSolicitud, CancellationToken cancellationToken);
        Task<Resultado<SolicitudDTO>> EnviarSolicitudIngresoAGrupo(EnviarSolicitudDTO solicitud, CancellationToken cancellationToken);
        Task<Resultado<IEnumerable<SolicitudDTO>>> ObtenerSolicitudesPorAdministrador(string idAdministrador, string estado, CancellationToken cancellationToken);
        Task<Resultado<bool>> IngresoAGrupoConCodigo(UnirseAGrupoDTO acceso, CancellationToken cancellationToken);
    }
}
