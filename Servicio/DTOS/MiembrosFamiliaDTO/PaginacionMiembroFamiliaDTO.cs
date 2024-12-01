using Servicio.DTOS.FamiliasDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.DTOS.MiembrosFamiliaDTO
{
    public class PaginacionMiembroFamiliaDTO
    {
        public IEnumerable<MiembroFamiliaDTO> Miembros { get; set; }
        public int TotalElementos { get; set; }
        public int TotalPaginas { get; set; }
        public int PaginaActual { get; set; }
        public int ElementosPorPagina { get; set; }
    }
}
