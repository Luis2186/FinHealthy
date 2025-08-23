using Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.DTOS.CategoriasDTO;
using Servicio.DTOS.SubCategoriasDTO;

namespace Servicio.S_Categorias
{
    public interface IServicioCategoria
    {
        Task<Resultado<CategoriaDTO>> ObtenerCategoriaPorId(int id, CancellationToken cancellationToken);
        Task<Resultado<IEnumerable<CategoriaDTO>>> ObtenerTodasLasCategorias(CancellationToken cancellationToken);
        Task<Resultado<CategoriaDTO>> CrearCategoria(CrearCategoriaDTO categoriaCreacionDTO, CancellationToken cancellationToken);
        Task<Resultado<CategoriaDTO>> ActualizarCategoria(int categoriaId, ActualizarCategoriaDTO categoriaActualizacionDTO, CancellationToken cancellationToken);
        Task<Resultado<bool>> EliminarCategoria(int id, CancellationToken cancellationToken);
    }
}