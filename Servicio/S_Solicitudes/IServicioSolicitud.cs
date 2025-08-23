using Servicio.DTOS.SolicitudesDTO;
using Dominio.Solicitudes;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio.S_Solicitudes
{
    public interface IServicioSolicitud : IServicioCrud<SolicitudDTO, CrearSolicitudDTO, ActualizarSolicitudDTO, SolicitudUnionGrupo>
    {
        // Métodos específicos pueden agregarse aquí si es necesario
    }
}
