using Dominio;
using Servicio.DTOS;
using Servicio.DTOS.GastosDTO;
using Dominio.Gastos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Servicio.S_Gastos
{
    public interface IServicioGasto
    {
        public Task<Resultado<GastoDTO>> CrearGasto(CrearGastoDTO gastoCreacionDTO, string usuarioActualId, CancellationToken cancellationToken);
        Task<Resultado<GastosSegmentadosDTO>> ObtenerGastosSegmentados(int grupoId, int? anio, int? mes, string usuarioActualId, TipoGasto tipoGasto, CancellationToken cancellationToken);
    }
}
