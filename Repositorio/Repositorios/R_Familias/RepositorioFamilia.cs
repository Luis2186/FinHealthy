using Dominio;
using Dominio.Abstracciones;
using Dominio.Errores;
using Dominio.Familias;
using Microsoft.EntityFrameworkCore;
using Repositorio.Repositorios.R_Familias;

namespace Repositorio.Repositorios.R_Familia
{
    public class RepositorioFamilia : IRepositorioFamilia
    {
        private readonly ApplicationDbContext _dbContext;
        public RepositorioFamilia(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Resultado<Familia>> ActualizarAsync(Familia model)
        {
            try
            {
                var familia = await ObtenerPorIdAsync(model.Id);

                if (familia.TieneErrores) return Resultado<Familia>.Failure(familia.Errores);

                var resultadoValidacion = DataAnnotationsValidator.Validar(model);

                if (resultadoValidacion.TieneErrores) return resultadoValidacion;

                _dbContext.Familias.Update(model);
                var resultadoActualizado = await _dbContext.SaveChangesAsync() == 1;

                if (!resultadoActualizado) return Resultado<Familia>.Failure(ErroresCrud.ErrorDeActualizacion("Familia"));

                return Resultado<Familia>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<Familia>.Failure(ErroresCrud.ErrorDeExcepcion("UPDATE", ex.Message));
            }
        }

        public async Task<Resultado<Familia>> CrearAsync(Familia model)
        {
            try
            {
                var resultadoValidacion = DataAnnotationsValidator.Validar(model);

                if (resultadoValidacion.TieneErrores) return resultadoValidacion;

                await _dbContext.Familias.AddAsync(model);
                var resultadoCreado = await _dbContext.SaveChangesAsync() == 1;

                if (!resultadoCreado) return Resultado<Familia>.Failure(ErroresCrud.ErrorDeCreacion("Familia"));

                return Resultado<Familia>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<Familia>.Failure(ErroresCrud.ErrorDeExcepcion("CREATE", ex.Message));
            };
        }

        public async Task<Resultado<bool>> EliminarAsync(int id)
        {
            try
            {
                var familia = await ObtenerPorIdAsync(id);

                if (familia.TieneErrores) return Resultado<bool>.Failure(familia.Errores);

                _dbContext.Familias.Remove(familia.Valor);
                var resultadoEliminado = await _dbContext.SaveChangesAsync() == 1;

                if (!resultadoEliminado) return Resultado<bool>.Failure(ErroresCrud.ErrorDeEliminacion("Familia"));

                return Resultado<bool>.Success(resultadoEliminado);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("REMOVE", ex.Message));
            }
        }

        public async Task<Resultado<bool>> MiembroExisteEnLaFamilia(int idFamilia, string usuarioId)
        {
            try
            {
                var familia = await ObtenerPorIdAsync(idFamilia);

                if (familia.TieneErrores) return Resultado<bool>.Failure(familia.Errores);

                MiembroFamilia miembroBuscado = familia.Valor.Miembros.FirstOrDefault(m => m.UsuarioId == usuarioId);

                if (miembroBuscado != null) return Resultado<bool>.Failure(ErroresFamilia.Error_Miembro_Existente("MiembroExisteEnLaFamilia"));

                return Resultado<bool>.Success(miembroBuscado==null);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("MiembroExisteEnLaFamilia", ex.Message));
            }
        }

        public async Task<Resultado<Familia>> ObtenerFamiliaPorIdAdministrador(string usuarioAdminId)
        {
            try
            {
                var familia = _dbContext.Familias
                    .Include(f => f.UsuarioAdministrador)
                    .Include(f => f.Miembros)
                    .ThenInclude(m => m.Usuario)
                    .FirstOrDefault(f => f.UsuarioAdministradorId == usuarioAdminId);

                if (familia == null) return Resultado<Familia>.Failure(ErroresCrud.ErrorBuscarPorId("Familia"));

                return Resultado<Familia>.Success(familia);
            }
            catch (Exception ex)
            {
                return Resultado<Familia>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_BY_ID", ex.Message));
            }
        }

        public async Task<Resultado<Familia>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var familia = _dbContext.Familias
                    .Include( f=> f.UsuarioAdministrador)
                    .Include(f => f.Miembros)
                    .ThenInclude(m => m.Usuario)
                    .FirstOrDefault(f => f.Id == id);

                if (familia == null) return Resultado<Familia>.Failure(ErroresCrud.ErrorBuscarPorId("Familia"));

                return Resultado<Familia>.Success(familia);
            }
            catch (Exception ex)
            {
                return Resultado<Familia>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_BY_ID", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<Familia>>> ObtenerTodosAsync()
        {
            try
            {
                var familias = await _dbContext.Familias
                    .Include(f => f.UsuarioAdministrador)
                    .Include(f => f.Miembros)
                    .ThenInclude(m => m.Usuario)
                    .ToListAsync();

                return Resultado<IEnumerable<Familia>>.Success(familias);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<Familia>>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_ALL", ex.Message));
            }
        }
    }
}
