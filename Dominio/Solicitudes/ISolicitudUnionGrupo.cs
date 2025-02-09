using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Solicitudes
{
    public interface ISolicitudUnionGrupo
    {
        Resultado<bool> Aceptar(SolicitudUnionGrupo solicitud);
        Resultado<bool> Rechazar(SolicitudUnionGrupo solicitud);
    }
}
