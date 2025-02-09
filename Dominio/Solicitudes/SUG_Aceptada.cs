using Dominio.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Solicitudes
{
    public class SUG_Aceptada : ISolicitudUnionGrupo
    {
        public Resultado<bool> Aceptar(SolicitudUnionGrupo solicitud)
        {
            solicitud.CambiarEstado("Aceptada");
            solicitud.FechaDeRespuesta = DateTime.Now;
            return Resultado<bool>.Success(true);
        }

        public Resultado<bool> Rechazar(SolicitudUnionGrupo solicitud)
        {
           return Resultado<bool>.Failure(new Error("SUGF_Aceptada.Rechazada", "No se puede rechazar una solicitud aceptada."));
        }
    }
}
