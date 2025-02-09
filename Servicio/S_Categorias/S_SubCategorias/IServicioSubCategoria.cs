using Dominio;
using Servicio.DTOS.SubCategoriasDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.S_Categorias.S_SubCategorias
{
    public interface IServicioSubCategoria : IServiciosCRUD<SubCategoriaDTO>
    {
        public Task<Resultado<IEnumerable<SubCategoriaDTO>>> ObtenerSubCategorias(int grupoId,int categoriaId);
    }
}
