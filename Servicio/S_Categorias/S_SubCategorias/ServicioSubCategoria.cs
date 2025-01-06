using AutoMapper;
using Dominio;
using Dominio.Gastos;
using Repositorio.Repositorios.R_Categoria;
using Repositorio.Repositorios.R_Categoria.R_SubCategoria;
using Repositorio.Repositorios.R_Familias;
using Servicio.DTOS.CategoriasDTO;
using Servicio.DTOS.SubCategoriasDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.S_Categorias.S_SubCategorias
{
    public class ServicioSubCategoria : IServicioSubCategoria
    {
        private readonly IRepositorioSubCategoria _repoSubCategoria;
        private readonly IRepositorioCategoria _repoCategoria;
        private readonly IRepositorioFamilia _repoFamilia;
        private readonly IMapper _mapper;
        public ServicioSubCategoria(IRepositorioSubCategoria repoSubCategoria, IMapper mapper,
            IRepositorioCategoria repoCategoria, IRepositorioFamilia repoFamilia)
        {
            _repoSubCategoria = repoSubCategoria;
            _mapper = mapper;
            _repoFamilia = repoFamilia;
            _repoCategoria = repoCategoria;
        }

        public async Task<Resultado<SubCategoriaDTO>> Actualizar(int idModel, SubCategoriaDTO model)
        {
            var subCategoriaBuscada = await _repoSubCategoria.ObtenerPorIdAsync(idModel);

            if (subCategoriaBuscada.TieneErrores) return Resultado<SubCategoriaDTO>.Failure(subCategoriaBuscada.Errores);

            var subCategoriaActDTO = _mapper.Map<ActualizarCategoriaDTO>(model);

            //mapeamos el DTO con los datos actualizados
            _mapper.Map(subCategoriaActDTO, subCategoriaBuscada.Valor);

            var subCategoriaActualizada = await _repoSubCategoria.ActualizarAsync(subCategoriaBuscada.Valor);

            if (subCategoriaActualizada.TieneErrores) return Resultado<SubCategoriaDTO>.Failure(subCategoriaBuscada.Errores);

            var subCategoriaActualizadaDTO = _mapper.Map<SubCategoriaDTO>(subCategoriaActualizada.Valor);

            return subCategoriaActualizadaDTO;
        }

        public async Task<Resultado<SubCategoriaDTO>> Crear(SubCategoriaDTO model)
        {

            var subCategoria = _mapper.Map<SubCategoria>(model);

            var resultadoFamilia = await _repoFamilia.ObtenerPorIdAsync(subCategoria.FamiliaId);
            var resultadoCategoria = await _repoCategoria.ObtenerPorIdAsync(subCategoria.CategoriaId);

            if (resultadoFamilia.TieneErrores) return Resultado<SubCategoriaDTO>.Failure(resultadoFamilia.Errores);
            if (resultadoCategoria.TieneErrores) return Resultado<SubCategoriaDTO>.Failure(resultadoCategoria.Errores);
            
            subCategoria.Familia = resultadoFamilia.Valor;
            subCategoria.Categoria = resultadoCategoria.Valor;

            var resultado = await _repoSubCategoria.CrearAsync(subCategoria);

            if (resultado.TieneErrores) return Resultado<SubCategoriaDTO>.Failure(resultado.Errores);

            var subCategoriaDTO = _mapper.Map<SubCategoriaDTO>(resultado.Valor);

            return subCategoriaDTO;

        }

        public async Task<Resultado<bool>> Eliminar(int id)
        {
            var resultado = await _repoSubCategoria.EliminarAsync(id);

            if (resultado.TieneErrores) return Resultado<bool>.Failure(resultado.Errores);

            return resultado;
        }

        public async Task<Resultado<SubCategoriaDTO>> ObtenerPorId(int id)
        {
            var resultado = await _repoSubCategoria.ObtenerPorIdAsync(id);

            if (resultado.TieneErrores) return Resultado<SubCategoriaDTO>.Failure(resultado.Errores);

            var subCategoriaDTO = _mapper.Map<SubCategoriaDTO>(resultado.Valor);

            return Resultado<SubCategoriaDTO>.Success(subCategoriaDTO);
        }

        public async Task<Resultado<IEnumerable<SubCategoriaDTO>>> ObtenerSubCategorias(int familiaId, int categoriaId)
        {
            var subCategorias = await _repoSubCategoria.ObtenerTodasPorFamiliaYCategoria(familiaId, categoriaId);

            if (subCategorias.TieneErrores) return Resultado<IEnumerable<SubCategoriaDTO>>.Failure(subCategorias.Errores);

            var subCategoriasDTO = _mapper.Map<IEnumerable<SubCategoriaDTO>>(subCategorias.Valor);

            return Resultado< IEnumerable<SubCategoriaDTO>>.Success(subCategoriasDTO);

        }

        public async Task<Resultado<IEnumerable<SubCategoriaDTO>>> ObtenerTodas()
        {
            throw new NotImplementedException();
        }
    }
}
