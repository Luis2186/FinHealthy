using AutoMapper;
using Dominio;
using Dominio.Gastos;
using Repositorio.Repositorios.R_Categoria;
using Repositorio.Repositorios.R_Categoria.R_SubCategoria;
using Repositorio.Repositorios.R_Grupo;
using Servicio.DTOS.SubCategoriasDTO;
using Servicio.DTOS.CategoriasDTO;
using Servicio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio.S_Categorias.S_SubCategorias
{
    public class ServicioSubCategoria : ServicioCrud<CrearSubCategoriaDTO, ActualizarCategoriaDTO, SubCategoriaDTO, SubCategoria>, IServicioSubCategoria
    {
        private readonly IRepositorioCategoria _repoCategoria;
        private readonly IRepositorioSubCategoria _repoSubCategoria;
        public ServicioSubCategoria(
            IRepositorioSubCategoria repo,
            IMapper mapper,
            IRepositorioCategoria repoCategoria,
            IRepositorioSubCategoria repoSubCategoria)
            : base(repo, mapper)
        {
            _repoCategoria = repoCategoria;
            _repoSubCategoria = repoSubCategoria;
        }

        public override async Task<Resultado<SubCategoriaDTO>> CrearAsync(CrearSubCategoriaDTO dto, CancellationToken cancellationToken)
        {
            var subCategoria = _mapper.Map<SubCategoria>(dto);
            var resultadoCategoria = await _repoCategoria.ObtenerPorIdAsync(subCategoria.CategoriaId, cancellationToken);
            if (resultadoCategoria.TieneErrores) return Resultado<SubCategoriaDTO>.Failure(resultadoCategoria.Errores);
            subCategoria.Categoria = resultadoCategoria.Valor;

            var resultado = await _repoSubCategoria.CrearAsync(subCategoria, cancellationToken);
            if (resultado.TieneErrores) return Resultado<SubCategoriaDTO>.Failure(resultado.Errores);
            var subCategoriaDTO = _mapper.Map<SubCategoriaDTO>(resultado.Valor);
            return subCategoriaDTO;
        }

        public Task<Resultado<SubCategoriaDTO>> ObtenerPorId(int id, CancellationToken cancellationToken)
            => ObtenerPorIdAsync(id, cancellationToken);

        public Task<Resultado<IEnumerable<SubCategoriaDTO>>> ObtenerTodas(CancellationToken cancellationToken)
            => ObtenerTodosAsync(cancellationToken);

        public Task<Resultado<SubCategoriaDTO>> Crear(CrearSubCategoriaDTO model, CancellationToken cancellationToken)
            => CrearAsync(model, cancellationToken);

        public Task<Resultado<SubCategoriaDTO>> Actualizar(int idModel, ActualizarCategoriaDTO model, CancellationToken cancellationToken)
            => ActualizarAsync(idModel, model, cancellationToken);

        public Task<Resultado<bool>> Eliminar(int id, CancellationToken cancellationToken)
            => EliminarAsync(id, cancellationToken);

        public async Task<Resultado<IEnumerable<SubCategoriaDTO>>> ObtenerSubCategorias(int grupoId, int categoriaId, CancellationToken cancellationToken)
        {
            var subCategorias = await _repoSubCategoria.ObtenerTodasPorGrupoYCategoria(grupoId, categoriaId, cancellationToken);
            if (subCategorias.TieneErrores) return Resultado<IEnumerable<SubCategoriaDTO>>.Failure(subCategorias.Errores);
            var subCategoriasDTO = _mapper.Map<IEnumerable<SubCategoriaDTO>>(subCategorias.Valor);
            return Resultado<IEnumerable<SubCategoriaDTO>>.Success(subCategoriasDTO);
        }
    }
}
