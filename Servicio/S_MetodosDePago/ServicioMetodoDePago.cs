using AutoMapper;
using Dominio.Gastos;
using Repositorio.Repositorios.R_Gastos.R_MetodosDePago;
using Servicio.DTOS.MetodosDePagoDTO;
using Servicio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio.S_MetodosDePago
{
    public class ServicioMetodoDePago : ServicioCrud<CrearMetodoDePagoDTO, ActualizarMetodoDePagoDTO, MetodoDePagoDTO, MetodoDePago>, IServicioMetodoDePago
    {
        public ServicioMetodoDePago(IRepositorioMetodoDePago repoMetodoDePago, IMapper mapper)
            : base(repoMetodoDePago, mapper)
        {
        }
        // Si necesitas lógica especial, haz override aquí
    }
}
