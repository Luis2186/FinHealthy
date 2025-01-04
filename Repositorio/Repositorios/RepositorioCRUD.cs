using Dominio;
using Dominio.Errores;
using Microsoft.EntityFrameworkCore;
using Repositorio.Repositorios.Validacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace Repositorio.Repositorios
{
    public class RepositorioCRUD<T> : IRepositorioCRUD<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidacion<T> _validacion;

        public RepositorioCRUD(ApplicationDbContext context, IValidacion<T> validacion)
        {
            _dbContext = context;
            _validacion = validacion;
        }
        public async Task<Resultado<T>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var entidad = await _dbContext.Set<T>().FindAsync(id);
                return entidad == null
                    ? Resultado<T>.Failure(ErroresCrud.ErrorDeCreacion(typeof(T).Name))
                    : Resultado<T>.Success(entidad);
            }
            catch (Exception ex)
            {
                return Resultado<T>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(T).Name}.ObtenerPorIdAsync", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<T>>> ObtenerTodosAsync()
        {
            try
            {
                var entidades = await _dbContext.Set<T>().ToListAsync();
                return Resultado<IEnumerable<T>>.Success(entidades);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<T>>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(T).Name}.ObtenerTodosAsync", ex.Message));
            }
        }

        public async Task<Resultado<T>> CrearAsync(T model)
        {
            try
            {
                var resultadoValidacion = _validacion.Validar(model);
                
                if (resultadoValidacion.TieneErrores) return resultadoValidacion;

                await _dbContext.Set<T>().AddAsync(model);
                await _dbContext.SaveChangesAsync();
                return Resultado<T>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<T>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(T).Name}.CrearAsync", ex.Message));
            }
        }

        public async Task<Resultado<T>> ActualizarAsync(T model)
        {
            try
            {
                var resultadoValidacion = _validacion.Validar(model);

                if (resultadoValidacion.TieneErrores) return resultadoValidacion;

                _dbContext.Set<T>().Update(model);
                await _dbContext.SaveChangesAsync();
                return Resultado<T>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<T>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(T).Name}.ActualizarAsync", ex.Message));
            }
        }

        public async Task<Resultado<bool>> EliminarAsync(int id)
        {
            try
            {
                var entidad = await ObtenerPorIdAsync(id);
                if (entidad.TieneErrores) return Resultado<bool>.Failure(entidad.Errores);

                _dbContext.Set<T>().Remove(entidad.Valor);

                await _dbContext.SaveChangesAsync();
                return Resultado<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(T).Name}.EliminarAsync", ex.Message));
            }
        }
    }
}
