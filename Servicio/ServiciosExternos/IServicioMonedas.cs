using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.ServiciosExternos
{
    public interface IServicioMonedas
    {
        public Task<Resultado<bool>> ActualizarMonedasDesdeServicio();
    }
}
