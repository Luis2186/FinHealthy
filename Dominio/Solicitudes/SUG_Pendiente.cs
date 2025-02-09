using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Solicitudes
{
    public class SUG_Pendiente : ISolicitudUnionGrupo
    {
        public Resultado<bool> Aceptar(SolicitudUnionGrupo solicitud)
        {
        
            solicitud.EstadoSolicitudGrupo = new SUG_Aceptada();

            return Resultado<bool>.Success(true);
        }

        public Resultado<bool> Rechazar(SolicitudUnionGrupo solicitud)
        {
          
            solicitud.EstadoSolicitudGrupo = new SUG_Rechazada();
            return Resultado<bool>.Success(true);
        }
    }
}
