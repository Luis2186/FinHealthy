using Dominio;
using Servicio.DTOS;
using Servicio.DTOS.GastosDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.S_Gastos
{
    public interface IServicioGasto
    {
        public Task<Resultado<GastoDTO>> CrearGasto(CrearGastoDTO gastoCreacionDTO);
    }
}
