using AutoMapper;
using Dominio;
using Dominio.Gastos;
using Repositorio.Repositorios.R_Categoria;
using Repositorio.Repositorios.R_Categoria.R_SubCategoria;
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
        private readonly IMapper _mapper;
        public ServicioSubCategoria(IRepositorioSubCategoria repoSubCategoria, IMapper mapper)
        {
            _repoSubCategoria = repoSubCategoria;
            _mapper = mapper;
        }

        public Task<Resultado<SubCategoriaDTO>> Actualizar(int idModel, SubCategoriaDTO model)
        {
            throw new NotImplementedException();
        }

        public async Task<Resultado<SubCategoriaDTO>> Crear(SubCategoriaDTO model)
        {
            var subCategoria = _mapper.Map<SubCategoria>(model);

            var resultado = await _repoSubCategoria.CrearAsync(subCategoria);

            if (resultado.TieneErrores) return Resultado<SubCategoriaDTO>.Failure(resultado.Errores);

            var subCategoriaDTO = _mapper.Map<SubCategoriaDTO>(resultado.Valor);

            return subCategoriaDTO;

        }

        public async Task<Resultado<bool>> Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Resultado<SubCategoriaDTO>> ObtenerPorId(int id)
        {
            throw new NotImplementedException();
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
