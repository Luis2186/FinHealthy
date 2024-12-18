﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Solicitudes
{
    public interface ISolicitudUnionGrupoFamiliar
    {
        Resultado<bool> Aceptar(SolicitudUnionFamilia solicitud);
        Resultado<bool> Rechazar(SolicitudUnionFamilia solicitud);
    }
}
