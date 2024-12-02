using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Solicitudes
{
    public abstract class EstadoSolicitudGrupoFamiliar
    {
        public abstract Resultado<bool> Aceptar(SolicitudUnionFamilia solicitud);
        public abstract Resultado<bool> Rechazar(SolicitudUnionFamilia solicitud);
    }
}
