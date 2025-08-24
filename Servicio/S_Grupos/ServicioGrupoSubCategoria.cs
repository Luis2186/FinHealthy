using Dominio.Grupos;
using Dominio.Abstracciones;
using Repositorio.Repositorios.R_Grupo;
using Servicio.DTOS.SubCategoriasDTO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Dominio;
using Repositorio.Repositorios.R_Categoria.R_SubCategoria;

namespace Servicio.S_Grupos
{

    public class ServicioGrupoSubCategoria : IServicioGrupoSubCategoria
    {
        private readonly IRepositorioGrupoSubCategoria _repoGrupoSubCategoria;
        private readonly IRepositorioSubCategoria _repoSubCategoria;

        public ServicioGrupoSubCategoria(IRepositorioGrupoSubCategoria repoGrupoSubCategoria, IRepositorioSubCategoria repoSubCategoria)
        {
            _repoGrupoSubCategoria = repoGrupoSubCategoria;
            _repoSubCategoria = repoSubCategoria;
        }

        public async Task<Resultado<List<GrupoSubCategoriaDTO>>> ObtenerPorGrupoIdAsync(int grupoId, CancellationToken cancellationToken)
        {
            var resultado = await _repoGrupoSubCategoria.ObtenerPorGrupoIdAsync(grupoId, cancellationToken);
            if (resultado.TieneErrores)
                return Resultado<List<GrupoSubCategoriaDTO>>.Failure(resultado.Errores);

            var dtos = resultado.Valor.Select(gsc => new GrupoSubCategoriaDTO
            {
                Id = gsc.Id,
                SubCategoriaId = gsc.SubCategoriaId,
                NombrePersonalizado = gsc.NombrePersonalizado,
                Color = gsc.Color,
                Activo = gsc.Activo
            }).ToList();

            return Resultado<List<GrupoSubCategoriaDTO>>.Success(dtos);
        }

        public async Task<Resultado<GrupoSubCategoriaDTO>> CrearAsync(int grupoId, CrearGrupoSubCategoriaDTO dto, CancellationToken cancellationToken)
        {
            var entidad = new GrupoSubCategoria
            {
                GrupoId = grupoId,
                SubCategoriaId = dto.SubCategoriaId,
                NombrePersonalizado = dto.NombrePersonalizado,
                Color = dto.Color,
                Activo = true
            };

            var resultado = await _repoGrupoSubCategoria.CrearAsync(entidad, cancellationToken);
            if (resultado.TieneErrores)
                return Resultado<GrupoSubCategoriaDTO>.Failure(resultado.Errores);

            var gsc = resultado.Valor;
            var dtoResult = new GrupoSubCategoriaDTO
            {
                Id = gsc.Id,
                SubCategoriaId = gsc.SubCategoriaId,
                NombrePersonalizado = gsc.NombrePersonalizado,
                Color = gsc.Color,
                Activo = gsc.Activo
            };
            return Resultado<GrupoSubCategoriaDTO>.Success(dtoResult);
        }

        public async Task<Resultado<GrupoSubCategoriaDTO>> ActualizarAsync(int grupoId, int id, ActualizarGrupoSubCategoriaDTO dto, CancellationToken cancellationToken)
        {
            var existente = await _repoGrupoSubCategoria.ObtenerPorIdAsync(id, cancellationToken);
            if (existente.TieneErrores || existente.Valor == null)
                return Resultado<GrupoSubCategoriaDTO>.Failure(existente.Errores);

            var entidad = existente.Valor;
            entidad.NombrePersonalizado = dto.NombrePersonalizado;
            entidad.Color = dto.Color;
            entidad.Activo = dto.Activo;

            var resultado = await _repoGrupoSubCategoria.ActualizarAsync(entidad, cancellationToken);
            if (resultado.TieneErrores)
                return Resultado<GrupoSubCategoriaDTO>.Failure(resultado.Errores);

            var gsc = resultado.Valor;
            var dtoResult = new GrupoSubCategoriaDTO
            {
                Id = gsc.Id,
                SubCategoriaId = gsc.SubCategoriaId,
                NombrePersonalizado = gsc.NombrePersonalizado,
                Color = gsc.Color,
                Activo = gsc.Activo
            };
            return Resultado<GrupoSubCategoriaDTO>.Success(dtoResult);
        }

        public async Task<Resultado<bool>> EliminarAsync(int grupoId, int id, CancellationToken cancellationToken)
        {
            var existente = await _repoGrupoSubCategoria.ObtenerPorIdAsync(id, cancellationToken);
            if (existente.TieneErrores || existente.Valor == null)
                return Resultado<bool>.Failure(existente.Errores);

            var resultado = await _repoGrupoSubCategoria.EliminarAsync(id, cancellationToken);
            if (resultado.TieneErrores)
                return Resultado<bool>.Failure(resultado.Errores);

            return Resultado<bool>.Success(true);
        }

        public async Task<Resultado<bool>> AsignarSubcategoriasBaseAlGrupoAsync(int grupoId, CancellationToken cancellationToken)
        {
            var subcategoriasBase = await _repoSubCategoria.ObtenerTodosAsync(cancellationToken);
            if (subcategoriasBase.TieneErrores)
                return Resultado<bool>.Failure(subcategoriasBase.Errores);

            foreach (var subCat in subcategoriasBase.Valor)
            {
                var grupoSubCat = new GrupoSubCategoria
                {
                    GrupoId = grupoId,
                    SubCategoriaId = subCat.Id,
                    NombrePersonalizado = subCat.Nombre,
                    Activo = true
                };
                await _repoGrupoSubCategoria.CrearAsync(grupoSubCat, cancellationToken);
            }

            return Resultado<bool>.Success(true);
        }
    }
}
