using Servicio.DTOS.SolicitudesDTO;
using Dominio.Solicitudes;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio.S_Solicitudes
{
    public interface IServicioSolicitud : IServicioCrud<SolicitudDTO, CrearSolicitudDTO, ActualizarSolicitudDTO, SolicitudUnionGrupo>
    {
        // M�todos espec�ficos pueden agregarse aqu� si es necesario
    }
}
