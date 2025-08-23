using AutoMapper;
using Dominio;
using Dominio.Gastos;
using Repositorio.Repositorios.R_Categoria;
using Servicio.DTOS.CategoriasDTO;
using Servicio.DTOS.SubCategoriasDTO;
using Servicio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio.S_Categorias
{
    public class ServicioCategoria : ServicioCrud<CrearCategoriaDTO, ActualizarCategoriaDTO, CategoriaDTO, Categoria>, IServicioCategoria
    {
        public ServicioCategoria(IRepositorioCategoria repoCategoria, IMapper mapper)
            : base(repoCategoria, mapper)
        {
        }

        public Task<Resultado<CategoriaDTO>> ObtenerCategoriaPorId(int id, CancellationToken cancellationToken)
            => ObtenerPorIdAsync(id, cancellationToken);

        public Task<Resultado<IEnumerable<CategoriaDTO>>> ObtenerTodasLasCategorias(CancellationToken cancellationToken)
            => ObtenerTodosAsync(cancellationToken);

        public Task<Resultado<CategoriaDTO>> CrearCategoria(CrearCategoriaDTO categoriaCreacionDTO, CancellationToken cancellationToken)
            => CrearAsync(categoriaCreacionDTO, cancellationToken);

        public Task<Resultado<CategoriaDTO>> ActualizarCategoria(int categoriaId, ActualizarCategoriaDTO categoriaActualizacionDTO, CancellationToken cancellationToken)
            => ActualizarAsync(categoriaId, categoriaActualizacionDTO, cancellationToken);

        public Task<Resultado<bool>> EliminarCategoria(int id, CancellationToken cancellationToken)
            => EliminarAsync(id, cancellationToken);
    }
}
