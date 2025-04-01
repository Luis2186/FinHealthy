using Dominio;
using Dominio.Abstracciones;
using Dominio.Errores;
using Dominio.Grupos;
using Microsoft.EntityFrameworkCore;

namespace Repositorio.Repositorios.R_Grupo
{
    public class RepositorioGrupo : IRepositorioGrupo
    {
        private readonly ApplicationDbContext _dbContext;
        public RepositorioGrupo(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Resultado<Grupo>> ActualizarAsync(Grupo model)
        {
            try
            {
                var grupo = await ObtenerPorIdAsync(model.Id);

                if (grupo.TieneErrores) return Resultado<Grupo>.Failure(grupo.Errores);

                var resultadoValidacion = DataAnnotationsValidator.Validar(model);

                if (resultadoValidacion.TieneErrores) return resultadoValidacion;

                _dbContext.GruposDeGasto.Update(model);
                var resultadoActualizado = await _dbContext.SaveChangesAsync() >= 1;

                if (!resultadoActualizado) return Resultado<Grupo>.Failure(ErroresCrud.ErrorDeActualizacion("Grupo"));

                return Resultado<Grupo>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<Grupo>.Failure(ErroresCrud.ErrorDeExcepcion("UPDATE", ex.Message));
            }
        }

        public async Task<Resultado<Grupo>> CrearAsync(Grupo model)
        {
            try
            {
                var resultadoValidacion = DataAnnotationsValidator.Validar(model);

                if (resultadoValidacion.TieneErrores) return resultadoValidacion;

                await _dbContext.GruposDeGasto.AddAsync(model);
                var resultadoCreado = await _dbContext.SaveChangesAsync() >= 1;

                if (!resultadoCreado) return Resultado<Grupo>.Failure(ErroresCrud.ErrorDeCreacion("Grupo"));

                return Resultado<Grupo>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<Grupo>.Failure(ErroresCrud.ErrorDeExcepcion("CREATE", ex.Message));
            };
        }

        public async Task<Resultado<bool>> EliminarAsync(int id)
        {
            try
            {
                var grupo = await ObtenerPorIdAsync(id);

                if (grupo.TieneErrores) return Resultado<bool>.Failure(grupo.Errores);

                _dbContext.GruposDeGasto.Remove(grupo.Valor);
                var resultadoEliminado = await _dbContext.SaveChangesAsync() >= 1;

                if (!resultadoEliminado) return Resultado<bool>.Failure(ErroresCrud.ErrorDeEliminacion("Grupo"));

                return Resultado<bool>.Success(resultadoEliminado);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("REMOVE", ex.Message));
            }
        }

        public async Task<Resultado<bool>> MiembroExisteEnElGrupo(int idGrupo, string usuarioId)
        {
            try
            {
                var grupo = await ObtenerPorIdAsync(idGrupo);

                if (grupo.TieneErrores) return Resultado<bool>.Failure(grupo.Errores);

                var miembroBuscado = grupo.Valor.MiembrosGrupoGasto.FirstOrDefault(m => m.Id == usuarioId);

                if (miembroBuscado != null) return Resultado<bool>.Failure(ErroresGrupo.Miembro_Existente("MiembroExisteEnElGrupo"));

                return Resultado<bool>.Success(miembroBuscado==null);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("MiembroExisteEnElGrupo", ex.Message));
            }
        }

        public async Task<Resultado<List<Grupo>>> ObtenerGruposPorUsuario(string usuarioId)
        {
            try
            {
                var grupo = _dbContext.GruposDeGasto
                    .Include(f => f.UsuarioAdministrador)
                    .Include(f => f.MiembrosGrupoGasto)
                    .Where(f => f.UsuarioAdministradorId == usuarioId
                    || f.MiembrosGrupoGasto.Any(m => m.Id == usuarioId))
                    .ToList();

                if (grupo == null) return Resultado<List<Grupo>>.Failure(ErroresCrud.ErrorBuscarPorId("Grupo"));

                return Resultado<List<Grupo>>.Success(grupo);
            }
            catch (Exception ex)
            {
                return Resultado<List<Grupo>>.Failure(ErroresCrud.ErrorDeExcepcion("ObtenerGruposPorUsuario", ex.Message));
            }
        }
        public async Task<Resultado<Grupo>> ObtenerGrupoPorIdAdministrador(string usuarioAdminId)
        {
            try
            {
                var grupo = _dbContext.GruposDeGasto
                    .Include(f => f.UsuarioAdministrador)
                    .Include(f => f.MiembrosGrupoGasto)
                    .FirstOrDefault(f => f.UsuarioAdministradorId == usuarioAdminId);

                if (grupo == null) return Resultado<Grupo>.Failure(ErroresCrud.ErrorBuscarPorId("Grupo"));

                return Resultado<Grupo>.Success(grupo);
            }
            catch (Exception ex)
            {
                return Resultado<Grupo>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_BY_ID", ex.Message));
            }
        }

        public async Task<Resultado<Grupo>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var grupo = _dbContext.GruposDeGasto
                    .Include( f=> f.UsuarioAdministrador)
                    .Include(f => f.MiembrosGrupoGasto)
                    .FirstOrDefault(f => f.Id == id);

                if (grupo == null) return Resultado<Grupo>.Failure(ErroresCrud.ErrorBuscarPorId("Grupo"));

                return Resultado<Grupo>.Success(grupo);
            }
            catch (Exception ex)
            {
                return Resultado<Grupo>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_BY_ID", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<Grupo>>> ObtenerTodosAsync()
        {
            try
            {
                var grupos = await _dbContext.GruposDeGasto
                    .Include(f => f.UsuarioAdministrador)
                    .Include(f => f.MiembrosGrupoGasto)
                    .ToListAsync();

                return Resultado<IEnumerable<Grupo>>.Success(grupos);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<Grupo>>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_ALL", ex.Message));
            }
        }
    }
}
