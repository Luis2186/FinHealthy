using Dominio;
using Dominio.Abstracciones;
using Dominio.Grupos;
using Microsoft.EntityFrameworkCore;
using Repositorio.Repositorios.Validacion;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.R_Grupo
{
    public class RepositorioGrupoSubCategoria : RepositorioCRUD<GrupoSubCategoria>, IRepositorioGrupoSubCategoria
    {
        private readonly ApplicationDbContext _context;

        public RepositorioGrupoSubCategoria(ApplicationDbContext context, IValidacion<GrupoSubCategoria> validacion) : base(context, validacion)
        {
            _context = context;
        }

        public async Task<Resultado<List<GrupoSubCategoria>>> ObtenerPorGrupoIdAsync(int grupoId, CancellationToken cancellationToken)
        {
            var lista = await _context.GrupoSubCategorias
                .Include(gsc => gsc.SubCategoria)
                .Where(gsc => gsc.GrupoId == grupoId && gsc.Activo)
                .ToListAsync(cancellationToken);

            return Resultado<List<GrupoSubCategoria>>.Success(lista);
        }

        public async Task<Resultado<GrupoSubCategoria>> ObtenerPorGrupoYSubCategoriaAsync(int grupoId, int subCategoriaId, CancellationToken cancellationToken)
        {
            var entidad = await _context.GrupoSubCategorias
                .Include(gsc => gsc.SubCategoria)
                .FirstOrDefaultAsync(gsc => gsc.GrupoId == grupoId && gsc.SubCategoriaId == subCategoriaId, cancellationToken);

            if (entidad == null)
                return Resultado<GrupoSubCategoria>.Failure(new Error("GrupoSubCategoria", "No existe la subcategoría en el grupo."));

            return Resultado<GrupoSubCategoria>.Success(entidad);
        }

        public async Task<Resultado<bool>> ExisteEnGrupoAsync(int grupoId, int subCategoriaId, CancellationToken cancellationToken)
        {
            var existe = await _context.GrupoSubCategorias
                .AnyAsync(gsc => gsc.GrupoId == grupoId && gsc.SubCategoriaId == subCategoriaId && gsc.Activo, cancellationToken);

            return Resultado<bool>.Success(existe);
        }
    }
}
