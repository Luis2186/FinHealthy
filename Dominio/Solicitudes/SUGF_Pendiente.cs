using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Solicitudes
{
    public class SUGF_Pendiente : ISolicitudUnionGrupoFamiliar
    {
        public Resultado<bool> Aceptar(SolicitudUnionGrupoFamiliar solicitud)
        {
            solicitud.CambiarEstado("Aceptada");
            solicitud.EstadoSolicitudGrupo = new SUGF_Aceptada();

            return Resultado<bool>.Success(true);
        }

        public Resultado<bool> Rechazar(SolicitudUnionGrupoFamiliar solicitud)
        {
            solicitud.CambiarEstado("Rechazada");
            solicitud.EstadoSolicitudGrupo = new SUGF_Rechazada();
            return Resultado<bool>.Success(true);
        }
    }
}
