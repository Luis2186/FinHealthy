using Servicio.DTOS.MetodosDePagoDTO;
using Dominio.Gastos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio.S_MetodosDePago
{
    public interface IServicioMetodoDePago : IServicioCrud<MetodoDePagoDTO, CrearMetodoDePagoDTO, ActualizarMetodoDePagoDTO, MetodoDePago>
    {
        // Puedes agregar métodos específicos aquí si lo necesitas
    }
}
