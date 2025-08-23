using Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.DTOS.SubCategoriasDTO;
using Servicio.DTOS.CategoriasDTO;

namespace Servicio.S_Categorias.S_SubCategorias
{
    public interface IServicioSubCategoria
    {
        Task<Resultado<SubCategoriaDTO>> ObtenerPorId(int id, CancellationToken cancellationToken);
        Task<Resultado<IEnumerable<SubCategoriaDTO>>> ObtenerTodas(CancellationToken cancellationToken);
        Task<Resultado<SubCategoriaDTO>> Crear(CrearSubCategoriaDTO model, CancellationToken cancellationToken);
        Task<Resultado<SubCategoriaDTO>> Actualizar(int idModel, ActualizarCategoriaDTO model, CancellationToken cancellationToken);
        Task<Resultado<bool>> Eliminar(int id, CancellationToken cancellationToken);
        Task<Resultado<IEnumerable<SubCategoriaDTO>>> ObtenerSubCategorias(int grupoId, int categoriaId, CancellationToken cancellationToken);
    }
}
