using Dominio;
using Servicio.DTOS.SubCategoriasDTO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio.S_Categorias.S_SubCategorias
{
    public interface IServicioSubCategoria : IServiciosCRUD<SubCategoriaDTO>
    {
        Task<Resultado<IEnumerable<SubCategoriaDTO>>> ObtenerSubCategorias(int grupoId, int categoriaId, CancellationToken cancellationToken);
        Task<Resultado<SubCategoriaDTO>> ObtenerPorId(int id, CancellationToken cancellationToken);
        Task<Resultado<IEnumerable<SubCategoriaDTO>>> ObtenerTodas(CancellationToken cancellationToken);
        Task<Resultado<SubCategoriaDTO>> Crear(SubCategoriaDTO model, CancellationToken cancellationToken);
        Task<Resultado<SubCategoriaDTO>> Actualizar(int idModel, SubCategoriaDTO model, CancellationToken cancellationToken);
        Task<Resultado<bool>> Eliminar(int id, CancellationToken cancellationToken);
    }
}
