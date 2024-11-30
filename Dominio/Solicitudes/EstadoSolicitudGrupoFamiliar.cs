using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Solicitudes
{
    public abstract class EstadoSolicitudGrupoFamiliar
    {
        public abstract Resultado<bool> Aceptar(SolicitudUnionGrupoFamiliar solicitud);
        public abstract Resultado<bool> Rechazar(SolicitudUnionGrupoFamiliar solicitud);
    }
}
