using Dominio.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Solicitudes
{
    public class SUGF_Rechazada : ISolicitudUnionGrupoFamiliar
    {
        public Resultado<bool> Aceptar(SolicitudUnionFamilia solicitud)
        {
            
            return Resultado<bool>.Failure(new Error("SUGF_Rechazada.Aceptada", "No se puede aceptar una solicitud rechazada."));
        }

        public Resultado<bool> Rechazar(SolicitudUnionFamilia solicitud)
        {
            solicitud.CambiarEstado("Rechazada");
            solicitud.FechaDeRespuesta = DateTime.Now;
            return Resultado<bool>.Success(true);

        }
    }
}
