using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Solicitudes
{
    public abstract class EstadoSolicitudGrupo
    {
        public abstract Resultado<bool> Aceptar(SolicitudUnionGrupo solicitud);
        public abstract Resultado<bool> Rechazar(SolicitudUnionGrupo solicitud);
    }
}
