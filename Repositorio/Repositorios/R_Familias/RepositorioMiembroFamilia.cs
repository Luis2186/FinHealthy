using Dominio;
using Dominio.Errores;
using Dominio.Familias;
using Microsoft.EntityFrameworkCore;
using Repositorio.Repositorios.R_Familias;

namespace Repositorio.Repositorios.R_Familia
{
    public class RepositorioMiembroFamilia : IRepositorioMiembroFamilia
    {
        private readonly ApplicationDbContext _dbContext;
        public RepositorioMiembroFamilia(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Resultado<MiembroFamilia>> ActualizarAsync(MiembroFamilia model)
        {
            try
            {
                var buscarMiembro = await ObtenerPorIdAsync(model.Id);

                if (buscarMiembro.TieneErrores) return Resultado<MiembroFamilia>.Failure(buscarMiembro.Errores);

                var resultadoValidacion = DataAnnotationsValidator.Validar(model);

                if (resultadoValidacion.TieneErrores) return resultadoValidacion;
                
                _dbContext.MiembrosFamiliares.Update(model);
                var resultadoActualizado = await _dbContext.SaveChangesAsync() == 1;

                if (!resultadoActualizado) return Resultado<MiembroFamilia>.Failure(ErroresCrud.ErrorDeActualizacion("Miembro Familiar"));
                
                return Resultado<MiembroFamilia>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<MiembroFamilia>.Failure(ErroresCrud.ErrorDeExcepcion("UPDATE",ex.Message));
            }
        }

        public async Task<Resultado<MiembroFamilia>> CrearAsync(MiembroFamilia model)
        {
            try
            {
                var resultadoValidacion = DataAnnotationsValidator.Validar(model);

                if (resultadoValidacion.TieneErrores) return resultadoValidacion;

                await _dbContext.MiembrosFamiliares.AddAsync(model);
                var resultadoCreado = await _dbContext.SaveChangesAsync() == 1;

                if (!resultadoCreado) return Resultado<MiembroFamilia>.Failure(ErroresCrud.ErrorDeCreacion("Miembro Familiar"));

                return Resultado<MiembroFamilia>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<MiembroFamilia>.Failure(ErroresCrud.ErrorDeExcepcion("CREATE", ex.Message));
            };
        }

        public async Task<Resultado<bool>> EliminarAsync(int id)
        {
            try
            {
                var buscarMiembro = await ObtenerPorIdAsync(id);

                if (buscarMiembro.TieneErrores) return Resultado<bool>.Failure(buscarMiembro.Errores);

                _dbContext.MiembrosFamiliares.Remove(buscarMiembro.Valor);
                var resultadoEliminado = await _dbContext.SaveChangesAsync() == 1;

                if (!resultadoEliminado) return Resultado<bool>.Failure(ErroresCrud.ErrorDeEliminacion("Miembro Familiar"));

                return Resultado<bool>.Success(resultadoEliminado);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("REMOVE", ex.Message));
            }
        }

        public async Task<Resultado<MiembroFamilia>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var miembro = _dbContext.MiembrosFamiliares.Find(id);

                if (miembro == null) return Resultado<MiembroFamilia>.Failure(ErroresCrud.ErrorBuscarPorId("Miembro Familiar"));

                return Resultado<MiembroFamilia>.Success(miembro);
            }
            catch (Exception ex)
            {
                return Resultado<MiembroFamilia>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_BY_ID", ex.Message));
            }
        }

        public async Task<Resultado<MiembroFamilia>> ObtenerPorUsuarioId(string id)
        {
            try
            {
                var miembro = await _dbContext.MiembrosFamiliares
                    .Include(m => m.Usuario)
                    .FirstOrDefaultAsync(m => m.UsuarioId == id);

                if (miembro == null) return Resultado<MiembroFamilia>.Failure(ErroresCrud.ErrorBuscarPorId("Miembro Familiar"));

                return Resultado<MiembroFamilia>.Success(miembro);
            }
            catch (Exception ex)
            {
                return Resultado<MiembroFamilia>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_BY_USUARIO_ID", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<MiembroFamilia>>> ObtenerTodosAsync()
        {
            try
            {
                var miembros = await _dbContext.MiembrosFamiliares
                    .Include(m => m.Usuario)
                    .ToListAsync();

                return Resultado<IEnumerable<MiembroFamilia>>.Success(miembros);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<MiembroFamilia>>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_ALL", ex.Message));
            }
        }
    }
}
