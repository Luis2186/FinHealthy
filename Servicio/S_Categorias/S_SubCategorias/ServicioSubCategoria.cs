using AutoMapper;
using Dominio;
using Dominio.Gastos;
using Repositorio.Repositorios.R_Categoria;
using Repositorio.Repositorios.R_Categoria.R_SubCategoria;
using Repositorio.Repositorios.R_Grupo;
using Servicio.DTOS.SubCategoriasDTO;
using System.Threading;

namespace Servicio.S_Categorias.S_SubCategorias
{
    public class ServicioSubCategoria : IServicioSubCategoria
    {
        private readonly IRepositorioSubCategoria _repoSubCategoria;
        private readonly IRepositorioCategoria _repoCategoria;
        private readonly IRepositorioGrupo _repoGrupo;
        private readonly IMapper _mapper;
        public ServicioSubCategoria(IRepositorioSubCategoria repoSubCategoria, IMapper mapper,
            IRepositorioCategoria repoCategoria, IRepositorioGrupo repoGrupo)
        {
            _repoSubCategoria = repoSubCategoria;
            _mapper = mapper;
            _repoGrupo = repoGrupo;
            _repoCategoria = repoCategoria;
        }

        public async Task<Resultado<SubCategoriaDTO>> Actualizar(int idModel, SubCategoriaDTO model, CancellationToken cancellationToken)
        {
            var subCategoriaBuscada = await _repoSubCategoria.ObtenerPorIdAsync(idModel, cancellationToken);

            if (subCategoriaBuscada.TieneErrores) return Resultado<SubCategoriaDTO>.Failure(subCategoriaBuscada.Errores);

            var subCategoriaActDTO = _mapper.Map<ActualizarCategoriaDTO>(model);

            //mapeamos el DTO con los datos actualizados
            _mapper.Map(subCategoriaActDTO, subCategoriaBuscada.Valor);

            var subCategoriaActualizada = await _repoSubCategoria.ActualizarAsync(subCategoriaBuscada.Valor, cancellationToken);

            if (subCategoriaActualizada.TieneErrores) return Resultado<SubCategoriaDTO>.Failure(subCategoriaBuscada.Errores);

            var subCategoriaActualizadaDTO = _mapper.Map<SubCategoriaDTO>(subCategoriaActualizada.Valor);

            return subCategoriaActualizadaDTO;
        }

        public async Task<Resultado<SubCategoriaDTO>> Crear(SubCategoriaDTO model, CancellationToken cancellationToken)
        {

            var subCategoria = _mapper.Map<SubCategoria>(model);

            var resultadoGrupo = await _repoGrupo.ObtenerPorIdAsync(subCategoria.GrupoId, cancellationToken);
            var resultadoCategoria = await _repoCategoria.ObtenerPorIdAsync(subCategoria.CategoriaId, cancellationToken);

            if (resultadoGrupo.TieneErrores) return Resultado<SubCategoriaDTO>.Failure(resultadoGrupo.Errores);
            if (resultadoCategoria.TieneErrores) return Resultado<SubCategoriaDTO>.Failure(resultadoCategoria.Errores);
            
            subCategoria.GrupoGasto = resultadoGrupo.Valor;
            subCategoria.Categoria = resultadoCategoria.Valor;

            var resultado = await _repoSubCategoria.CrearAsync(subCategoria, cancellationToken);

            if (resultado.TieneErrores) return Resultado<SubCategoriaDTO>.Failure(resultado.Errores);

            var subCategoriaDTO = _mapper.Map<SubCategoriaDTO>(resultado.Valor);

            return subCategoriaDTO;

        }

        public async Task<Resultado<bool>> Eliminar(int id, CancellationToken cancellationToken)
        {
            var resultado = await _repoSubCategoria.EliminarAsync(id, cancellationToken);

            if (resultado.TieneErrores) return Resultado<bool>.Failure(resultado.Errores);

            return resultado;
        }

        public async Task<Resultado<SubCategoriaDTO>> ObtenerPorId(int id, CancellationToken cancellationToken)
        {
            var resultado = await _repoSubCategoria.ObtenerPorIdAsync(id, cancellationToken);

            if (resultado.TieneErrores) return Resultado<SubCategoriaDTO>.Failure(resultado.Errores);

            var subCategoriaDTO = _mapper.Map<SubCategoriaDTO>(resultado.Valor);

            return Resultado<SubCategoriaDTO>.Success(subCategoriaDTO);
        }

        public async Task<Resultado<IEnumerable<SubCategoriaDTO>>> ObtenerSubCategorias(int grupoId, int categoriaId, CancellationToken cancellationToken)
        {
            var subCategorias = await _repoSubCategoria.ObtenerTodasPorGrupoYCategoria(grupoId, categoriaId, cancellationToken);

            if (subCategorias.TieneErrores) return Resultado<IEnumerable<SubCategoriaDTO>>.Failure(subCategorias.Errores);

            var subCategoriasDTO = _mapper.Map<IEnumerable<SubCategoriaDTO>>(subCategorias.Valor);

            return Resultado< IEnumerable<SubCategoriaDTO>>.Success(subCategoriasDTO);

        }

        public async Task<Resultado<IEnumerable<SubCategoriaDTO>>> ObtenerTodas(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
