using Dominio.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Solicitudes
{
    public class ErroresSolicitud
    {
        public static readonly Error Estado_No_Es_Pendiente = new Error("", "La solicitud no se encuentra en estado Pendiente, por favor revisela");




    }
}
