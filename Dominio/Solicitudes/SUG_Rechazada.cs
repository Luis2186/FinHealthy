using Dominio.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Solicitudes
{
    public class SUG_Rechazada : ISolicitudUnionGrupo
    {
        public Resultado<bool> Aceptar(SolicitudUnionGrupo solicitud)
        {
            
            return Resultado<bool>.Failure(new Error("SUGF_Rechazada.Aceptada", "No se puede aceptar una solicitud rechazada."));
        }

        public Resultado<bool> Rechazar(SolicitudUnionGrupo solicitud)
        {
            solicitud.CambiarEstado("Rechazada");
            solicitud.FechaDeRespuesta = DateTime.Now;
            return Resultado<bool>.Success(true);

        }
    }
}
