using AutoMapper;
using Dominio;
using Repositorio.Repositorios.R_Categoria;
using Servicio.DTOS.CategoriasDTO;
using Servicio.DTOS.SubCategoriasDTO;

namespace Servicio.S_Categorias
{
    public class ServicioCategoria : IServicioCategoria
    {
        private readonly IRepositorioCategoria _repoCategoria;
        private readonly IMapper _mapper;
        public ServicioCategoria(IRepositorioCategoria repoCategoria, IMapper mapper)
        {
            _repoCategoria = repoCategoria;
            _mapper = mapper;
        }

        public Task<Resultado<CategoriaDTO>> ActualizarCategoria(int categoriaId, CategoriaDTO categoriaActualizacionDTO)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado<CategoriaDTO>> CrearCategoria(CategoriaDTO categoriaCreacionDTO)
        {
            throw new NotImplementedException();
        }

        public Task<Resultado<bool>> EliminarCategoria(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Resultado<CategoriaDTO>> ObtenerCategoriaPorId(int id)
        {
            var resultado = await _repoCategoria.ObtenerPorIdAsync(id);

            if (resultado.TieneErrores) return Resultado<CategoriaDTO>.Failure(resultado.Errores);

            var subCategoriaDTO = _mapper.Map<CategoriaDTO>(resultado.Valor);

            return Resultado<CategoriaDTO>.Success(subCategoriaDTO);
        }

        public async Task<Resultado<IEnumerable<CategoriaDTO>>> ObtenerTodasLasCategorias()
        {
            var resultado = await _repoCategoria.ObtenerTodosAsync();

            if (resultado.TieneErrores) return Resultado<IEnumerable<CategoriaDTO>>.Failure(resultado.Errores);

            var categorias = resultado.Valor;

            var categoriasDTO = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

            return Resultado<IEnumerable<CategoriaDTO>>.Success(categoriasDTO);
        }
    }
}
