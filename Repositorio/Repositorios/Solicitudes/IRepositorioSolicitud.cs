using Dominio;
using Dominio.Solicitudes;
using Repositorio.Repositorios;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.Solicitudes
{
    public interface IRepositorioSolicitud : IRepositorioCRUD<SolicitudUnionGrupo>
    {
        Task<Resultado<bool>> AceptarSolicitud(int idSolicitud, CancellationToken cancellationToken);
        Task<Resultado<bool>> RechazarSolicitud(int idSolicitud, CancellationToken cancellationToken);
        Task<Resultado<IEnumerable<SolicitudUnionGrupo>>> ObtenerTodasPorAdministrador(string idAdministrador, string estado, CancellationToken cancellationToken);
    }
}
