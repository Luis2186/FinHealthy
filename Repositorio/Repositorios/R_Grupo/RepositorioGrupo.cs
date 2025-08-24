using Dominio;
using Dominio.Grupos;
using Dominio.Errores;
using Dominio.Abstracciones;
using Repositorio.Repositorios.Validacion;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Repositorio.Repositorios.R_Grupo
{
    public class RepositorioGrupo : RepositorioCRUD<Grupo>, IRepositorioGrupo
    {
        public RepositorioGrupo(ApplicationDbContext context, IValidacion<Grupo> validacion)
            : base(context, validacion)
        {
        }

        // Métodos específicos de grupo
        public async Task<Resultado<bool>> MiembroExisteEnElGrupo(int idGrupo, string usuarioId)
        {
            var grupo = await ObtenerPorIdAsync(idGrupo, CancellationToken.None);
            if (grupo.TieneErrores) return Resultado<bool>.Failure(grupo.Errores);
            var miembroBuscado = grupo.Valor.MiembrosGrupoGasto.FirstOrDefault(m => m.Id == usuarioId);
            if (miembroBuscado != null) return Resultado<bool>.Failure(ErroresGrupo.Miembro_Existente("MiembroExisteEnElGrupo"));
            return Resultado<bool>.Success(miembroBuscado == null);
        }

        public async Task<Resultado<List<Grupo>>> ObtenerGruposPorUsuario(string usuarioId)
        {
            var grupos = await _dbContext.GruposDeGasto
                .Include(f => f.UsuarioAdministrador)
                .Include(f => f.MiembrosGrupoGasto)
                .Where(f => f.UsuarioAdministradorId == usuarioId || f.MiembrosGrupoGasto.Any(m => m.Id == usuarioId))
                .ToListAsync();
            if (grupos == null) return Resultado<List<Grupo>>.Failure(ErroresCrud.ErrorBuscarPorId("Grupo"));
            return Resultado<List<Grupo>>.Success(grupos);
        }

        public async Task<Resultado<Grupo>> ObtenerGrupoPorIdAdministrador(string usuarioAdminId)
        {
            var grupo = await _dbContext.GruposDeGasto
                .Include(f => f.UsuarioAdministrador)
                .Include(f => f.MiembrosGrupoGasto)
                .FirstOrDefaultAsync(f => f.UsuarioAdministradorId == usuarioAdminId);
            if (grupo == null) return Resultado<Grupo>.Failure(ErroresCrud.ErrorBuscarPorId("Grupo"));
            return Resultado<Grupo>.Success(grupo);
        }

        public async Task<Resultado<Grupo>> ObtenerGrupoPorIdAdministrador(string usuarioAdminId, CancellationToken cancellationToken)
        {
            try
            {
                var grupo = await _dbContext.GruposDeGasto
                    .Include(g => g.MiembrosGrupoGasto)
                    .FirstOrDefaultAsync(g => g.UsuarioAdministradorId == usuarioAdminId, cancellationToken);
                if (grupo == null)
                    return Resultado<Grupo>.Failure(new Error("GRUPO_NO_ENCONTRADO", "No se encontró el grupo para el administrador."));
                return Resultado<Grupo>.Success(grupo);
            }
            catch (Exception ex)
            {
                return Resultado<Grupo>.Failure(new Error("ERROR_OBTENER_GRUPO_ADMIN", ex.Message));
            }
        }

        public async Task<Resultado<Grupo>> ObtenerGrupoPorIdConUsuariosYSubcategorias(int grupoId, CancellationToken cancellationToken)
        {
            var grupo = await _dbContext.GruposDeGasto
                .Include(g => g.UsuarioAdministrador)
                .Include(g => g.MiembrosGrupoGasto)
                .Include(g => g.GrupoSubCategorias)
                    .ThenInclude(gsc => gsc.SubCategoria)
                .FirstOrDefaultAsync(g => g.Id == grupoId, cancellationToken);

            if (grupo == null)
                return Resultado<Grupo>.Failure(new Error("GRUPO_NO_ENCONTRADO", "No se encontró el grupo con el id especificado."));

            return Resultado<Grupo>.Success(grupo);
        }
    }
}
