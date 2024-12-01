using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.DTOS.FamiliasDTO
{
    public class FamiliaPaginacionDTO
    {
        public IEnumerable<FamiliaDTO> Usuarios { get; set; }
        public int TotalElementos { get; set; }
        public int TotalPaginas { get; set; }
        public int PaginaActual { get; set; }
        public int ElementosPorPagina { get; set; }
    }
}
