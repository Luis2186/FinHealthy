using Dominio.Gastos;
using Servicio.DTOS.SubCategoriasDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.DTOS.CategoriasDTO
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public List<SubCategoriaDTO> SubCategorias { get; set; } = new List<SubCategoriaDTO>();


    }
}
