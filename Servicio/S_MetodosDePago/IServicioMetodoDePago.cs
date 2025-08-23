using Servicio.DTOS.MetodosDePagoDTO;
using Dominio.Gastos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio.S_MetodosDePago
{
    public interface IServicioMetodoDePago : IServicioCrud<MetodoDePagoDTO, CrearMetodoDePagoDTO, ActualizarMetodoDePagoDTO, MetodoDePago>
    {
        // Puedes agregar m�todos espec�ficos aqu� si lo necesitas
    }
}
