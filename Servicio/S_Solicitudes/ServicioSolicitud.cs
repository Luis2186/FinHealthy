using AutoMapper;
using Dominio.Solicitudes;
using Repositorio.Repositorios.Solicitudes;
using Servicio.DTOS.SolicitudesDTO;
using Servicio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio.S_Solicitudes
{
    public class ServicioSolicitud : ServicioCrud<CrearSolicitudDTO, ActualizarSolicitudDTO, SolicitudDTO, SolicitudUnionGrupo>
    {
        public ServicioSolicitud(IRepositorioSolicitud repoSolicitud, IMapper mapper)
            : base(repoSolicitud, mapper)
        {
        }
        // Si necesitas lógica especial, haz override aquí
    }
}
