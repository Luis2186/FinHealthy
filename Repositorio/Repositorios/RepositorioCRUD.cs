using Dominio;
using Dominio.Errores;
using Microsoft.EntityFrameworkCore;
using Repositorio.Repositorios.Validacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Repositorio.Repositorios
{
    public class RepositorioCRUD<T> : IRepositorioCRUD<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;
        private readonly IValidacion<T> _validacion;

        public RepositorioCRUD(ApplicationDbContext context, IValidacion<T> validacion)
        {
            _dbContext = context;
            _validacion = validacion;
        }

        public async Task<Resultado<T>> ObtenerPorIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var entidad = await _dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);
                return entidad == null
                    ? Resultado<T>.Failure(ErroresCrud.ErrorBuscarPorId(typeof(T).Name))
                    : Resultado<T>.Success(entidad);
            }
            catch (Exception ex)
            {
                return Resultado<T>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(T).Name}.ObtenerPorIdAsync", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<T>>> ObtenerTodosAsync(CancellationToken cancellationToken)
        {
            try
            {
                var entidades = await _dbContext.Set<T>().ToListAsync(cancellationToken);
                return Resultado<IEnumerable<T>>.Success(entidades);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<T>>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(T).Name}.ObtenerTodosAsync", ex.Message));
            }
        }

        public async Task<Resultado<T>> CrearAsync(T model, CancellationToken cancellationToken)
        {
            var resultadoValidacion = _validacion.Validar(model);
            if (resultadoValidacion.TieneErrores)
                return Resultado<T>.Failure(resultadoValidacion.Errores);

            try
            {
                await _dbContext.Set<T>().AddAsync(model, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return Resultado<T>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<T>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(T).Name}.CrearAsync", ex.Message));
            }
        }

        public async Task<Resultado<T>> ActualizarAsync(T model, CancellationToken cancellationToken)
        {
            var resultadoValidacion = _validacion.Validar(model);
            if (resultadoValidacion.TieneErrores)
                return Resultado<T>.Failure(resultadoValidacion.Errores);

            try
            {
                _dbContext.Set<T>().Update(model);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return Resultado<T>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<T>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(T).Name}.ActualizarAsync", ex.Message));
            }
        }

        public async Task<Resultado<bool>> EliminarAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var entidad = await ObtenerPorIdAsync(id, cancellationToken);
                if (entidad.TieneErrores) return Resultado<bool>.Failure(entidad.Errores);

                _dbContext.Set<T>().Remove(entidad.Valor);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return Resultado<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion($"{typeof(T).Name}.EliminarAsync", ex.Message));
            }
        }

        public async Task<Resultado<(IEnumerable<T> Items, int TotalItems)>> ObtenerPaginadoAsync(
            Func<IQueryable<T>, IQueryable<T>> filtro,
            int pagina,
            int tamanoPagina,
            string campoOrden,
            string direccionOrden,
            CancellationToken cancellationToken)
        {
            var query = filtro(_dbContext.Set<T>().AsNoTracking());
            var prop = typeof(T).GetProperty(campoOrden, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (prop != null)
            {
                query = direccionOrden.ToLower() == "desc"
                    ? query.OrderByDescending(x => EF.Property<object>(x, prop.Name))
                    : query.OrderBy(x => EF.Property<object>(x, prop.Name));
            }
            var totalItems = await query.CountAsync(cancellationToken);
            var items = await query.Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina).ToListAsync(cancellationToken);
            return Resultado<(IEnumerable<T> Items, int TotalItems)>.Success((items, totalItems));
        }
    }
}
