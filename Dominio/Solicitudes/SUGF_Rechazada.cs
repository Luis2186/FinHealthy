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
        public Resultado<bool> Aceptar(SolicitudUnionGrupoFamiliar solicitud)
        {
            return Resultado<bool>.Failure(new Error("SUGF_Rechazada.Aceptada", "No se puede aceptar una solicitud rechazada."));
        }

        public Resultado<bool> Rechazar(SolicitudUnionGrupoFamiliar solicitud)
        {
            throw new NotImplementedException();
        }
    }
}
