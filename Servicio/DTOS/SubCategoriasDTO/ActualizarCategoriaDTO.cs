using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.DTOS.SubCategoriasDTO
{
    public class ActualizarCategoriaDTO 
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int CategoriaId { get; set; }
        public int FamiliaId { get; set; }
    }
}
